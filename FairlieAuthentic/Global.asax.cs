using System;
using System.Collections.Generic;
using System.Configuration;
using System.IdentityModel.Metadata;
using System.IdentityModel.Tokens;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Security.Claims;
using System.Security.Cryptography.X509Certificates;
using System.Security.Principal;
using System.ServiceModel.Security;
using System.Threading;
using System.Threading.Tasks;
using System.Web;
using System.Web.Http;
using System.Web.Mvc;
using System.Web.Optimization;
using System.Web.Routing;
using System.Web.Security;
using System.Xml;
using Microsoft.IdentityModel.Tokens.JWT;
using Mindscape.LightSpeed;
using Mindscape.LightSpeed.MetaData;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;

namespace FairlieAuthentic
{
    // Note: For instructions on enabling IIS6 or IIS7 classic mode, 
    // visit http://go.microsoft.com/?LinkId=9394801

    public class WebApiApplication : System.Web.HttpApplication
    {
        protected void Application_Start()
        {
            AreaRegistration.RegisterAllAreas();

            WebApiConfig.Register(GlobalConfiguration.Configuration);
            FilterConfig.RegisterGlobalFilters(GlobalFilters.Filters);
            RouteConfig.RegisterRoutes(RouteTable.Routes);
            BundleConfig.RegisterBundles(BundleTable.Bundles);
        }
    }

    public class EntityContractResolver : DefaultContractResolver
    {
        private const string IdFieldName = "Id";
        private readonly string[] NonSerializedProperties = new string[5] { "Errors", "Error", "IsValid", "EntityState", "ChangeTracker" };
        private readonly string[] UnignoreProperties = new string[3] { "CreatedOn", "UpdatedOn", "DeletedOn" };

        protected override JsonContract CreateContract(Type objectType)
        {
            if (typeof(Entity).IsAssignableFrom(objectType))
            {
                var contract = base.CreateContract(objectType) as JsonObjectContract;

                foreach (var property in contract.Properties.ToList())
                {
                    if (property.PropertyName.StartsWith("_") || NonSerializedProperties.Contains(property.PropertyName))
                    {
                        contract.Properties.Remove(property);
                    }

                    if (UnignoreProperties.Contains(property.PropertyName) || !property.PropertyName.StartsWith("_"))
                    {
                        property.Ignored = false;
                    }
                }

                var entityInfo = EntityInfo.FromType(objectType);
                var idField = entityInfo.Fields.SingleOrDefault(f => f.PropertyName == IdFieldName);

                contract.Properties.Add(new JsonProperty()
                {
                    PropertyName = IdFieldName,
                    PropertyType = idField.FieldType,
                    Readable = true,
                    Writable = false,
                    Required = Required.Default,
                    DeclaringType = idField.Field.DeclaringType,
                    UnderlyingName = IdFieldName,
                    ValueProvider = new DynamicValueProvider(idField.Field)
                });


                return contract;
            }
            else
            {
                return base.CreateContract(objectType);
            }
        }
    }

    internal class TokenValidationHandler : DelegatingHandler
    {
        private static bool TryRetrieveToken(HttpRequestMessage request, out string token)
        {
            token = null;
            IEnumerable<string> authzHeaders;
            if (!request.Headers.TryGetValues("Authorization", out authzHeaders) || authzHeaders.Count() > 1)
            {
                return false;
            }
            var bearerToken = authzHeaders.ElementAt(0);
            token = bearerToken.StartsWith("Bearer ") ? bearerToken.Substring(7) : bearerToken;
            return true;
        }

        protected override Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken cancellationToken)
        {
            HttpStatusCode statusCode;
            string token;
            var authorisationHeader = request.Headers.Authorization;

            if (!TryRetrieveToken(request, out token))
            {
                statusCode = HttpStatusCode.Unauthorized;
                return Task<HttpResponseMessage>.Factory.StartNew(() => new HttpResponseMessage(statusCode));
            }
            try
            {
                X509SecurityToken cert = new X509SecurityToken(new X509Certificate2(
                    GetSigningCertificate(ConfigurationManager.AppSettings["ida:FederationMetadataLocation"])));

                JWTSecurityTokenHandler tokenHandler = new JWTSecurityTokenHandler();
                TokenValidationParameters validationParameters = new TokenValidationParameters()
                    {
                        AllowedAudience = ConfigurationManager.AppSettings["fa:AllowedAudience"],
                        ValidIssuer = "https://fairlieauthentic.accesscontrol.windows.net/",
                        SigningToken = cert
                    };

                ClaimsPrincipal cp = tokenHandler.ValidateToken(token, validationParameters);
                Thread.CurrentPrincipal = cp;

                return base.SendAsync(request, cancellationToken);
            }
            catch (SecurityTokenValidationException e)
            {
                statusCode = HttpStatusCode.Unauthorized;
            }
            catch (Exception)
            {
                statusCode = HttpStatusCode.InternalServerError;
            }
            return Task<HttpResponseMessage>.Factory.StartNew(() => new HttpResponseMessage(statusCode));
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

    internal class  UserValidationHandler : DelegatingHandler
    {
        protected override Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken cancellationToken)
        {
            var user = Thread.CurrentPrincipal;
            if (user == null)
            {
                return Task<HttpResponseMessage>.Factory.StartNew(() => new HttpResponseMessage(HttpStatusCode.NoContent));
            }
            string[] roles = Roles.Provider.GetRolesForUser(user.Identity.Name);

            var principal = new GenericPrincipal(user.Identity, roles);
            HttpContext.Current.User = principal;
            if (!principal.IsInRole("User"))
            {
                return Task<HttpResponseMessage>.Factory.StartNew(() => new HttpResponseMessage(HttpStatusCode.NoContent));
            }
            return base.SendAsync(request, cancellationToken);
        }
    }
}