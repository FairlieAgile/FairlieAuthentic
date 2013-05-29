using System;
using System.Collections.Generic;
using System.Configuration;
using System.IdentityModel.Metadata;
using System.IdentityModel.Tokens;
using System.Linq;
using System.Security.Claims;
using System.Security.Cryptography.X509Certificates;
using System.ServiceModel.Security;
using System.Web;
using System.Xml;
using Microsoft.IdentityModel.Tokens.JWT;

namespace FairlieAuthenticClientJWT.Infrastructure
{
    public class CustomJwtSecurityTokenHandler : JWTSecurityTokenHandler
    {
        public override ClaimsPrincipal ValidateToken(JWTSecurityToken jwt, TokenValidationParameters validationParameters)
        {
            if ((validationParameters.ValidIssuer == null) && (validationParameters.ValidIssuers == null || !validationParameters.ValidIssuers.Any()))
            {
                //validationParameters.ValidIssuers = new List<string>(((ValidatingIssuerNameRegistry)Configuration.IssuerNameRegistry).IssuingAuthorities.First().Issuers);
                validationParameters.ValidIssuers = new List<string> { ((ConfigurationBasedIssuerNameRegistry)Configuration.IssuerNameRegistry).ConfiguredTrustedIssuers.First().Value }; 

            }

            if (validationParameters.SigningToken == null)
            {
                validationParameters.SigningToken = new X509SecurityToken(new X509Certificate2(
                    GetSigningCertificate(ConfigurationManager.AppSettings["ida:FederationMetadataLocation"])));
            }
            return base.ValidateToken(jwt, validationParameters);
        }

        protected override string NameIdentifierClaimType(JWTSecurityToken jwt)
        {
            return ClaimTypes.GivenName;
        }

        public static byte[] GetSigningCertificate(string metadataAddress)
        {
            if (metadataAddress == null)
            {
                throw new ArgumentNullException(metadataAddress);
            }

            using (XmlReader metadataReader = XmlReader.Create(metadataAddress))
            {
                MetadataSerializer serializer = new MetadataSerializer()
                {
                    CertificateValidationMode = X509CertificateValidationMode.None
                };

                EntityDescriptor metadata = serializer.ReadMetadata(metadataReader) as EntityDescriptor;

                if (metadata != null)
                {
                    SecurityTokenServiceDescriptor stsd = metadata.RoleDescriptors.OfType<SecurityTokenServiceDescriptor>().First();

                    if (stsd != null)
                    {
                        X509RawDataKeyIdentifierClause clause = stsd.Keys.First().KeyInfo.OfType<X509RawDataKeyIdentifierClause>().First();

                        if (clause != null)
                        {
                            return clause.GetX509RawData();
                        }
                        throw new Exception("The SecurityTokenServiceDescriptor in the metadata does not contain the Signing Certificate in the <X509Certificate> element");
                    }
                    throw new Exception("The Federation Metadata document does not contain a SecurityTokenServiceDescriptor");
                }
                throw new Exception("Invalid Federation Metadata document");
            }
        }
    } 

}