using System;

using Mindscape.LightSpeed;
using Mindscape.LightSpeed.Validation;
using Mindscape.LightSpeed.Linq;

namespace FairlieAuthentic.Models
{
  [Serializable]
  [System.CodeDom.Compiler.GeneratedCode("LightSpeedModelGenerator", "1.0.0.0")]
  [System.ComponentModel.DataObject]
  public partial class Customer : Entity<int>
  {
    #region Fields
  
    private string _username;
    private string _email;

    #pragma warning disable 649  // "Field is never assigned to" - LightSpeed assigns these fields internally
    private readonly System.DateTime _createdOn;
    private readonly System.DateTime _updatedOn;
    #pragma warning restore 649    

    #endregion
    
    #region Field attribute and view names
    
    /// <summary>Identifies the Username entity attribute.</summary>
    public const string UsernameField = "Username";
    /// <summary>Identifies the Email entity attribute.</summary>
    public const string EmailField = "Email";
    /// <summary>Identifies the CreatedOn entity attribute.</summary>
    public const string CreatedOnField = "CreatedOn";
    /// <summary>Identifies the UpdatedOn entity attribute.</summary>
    public const string UpdatedOnField = "UpdatedOn";


    #endregion
    
    #region Relationships

    [ReverseAssociation("Customer")]
    private readonly EntityCollection<Identity> _identities = new EntityCollection<Identity>();
    [ReverseAssociation("Customer")]
    private readonly EntityCollection<CustomerRole> _customerRoles = new EntityCollection<CustomerRole>();

    private ThroughAssociation<CustomerRole, Role> _roles;

    #endregion
    
    #region Properties

    [System.Diagnostics.DebuggerNonUserCode]
    public EntityCollection<Identity> Identities
    {
      get { return Get(_identities); }
    }

    [System.Diagnostics.DebuggerNonUserCode]
    public EntityCollection<CustomerRole> CustomerRoles
    {
      get { return Get(_customerRoles); }
    }

    [System.Diagnostics.DebuggerNonUserCode]
    public ThroughAssociation<CustomerRole, Role> Roles
    {
      get
      {
        if (_roles == null)
        {
          _roles = new ThroughAssociation<CustomerRole, Role>(_customerRoles);
        }
        return Get(_roles);
      }
    }
    

    [System.Diagnostics.DebuggerNonUserCode]
    public string Username
    {
      get { return Get(ref _username, "Username"); }
      set { Set(ref _username, value, "Username"); }
    }

    [System.Diagnostics.DebuggerNonUserCode]
    public string Email
    {
      get { return Get(ref _email, "Email"); }
      set { Set(ref _email, value, "Email"); }
    }
    /// <summary>Gets the time the entity was created</summary>
    [System.Diagnostics.DebuggerNonUserCode]
    public System.DateTime CreatedOn
    {
      get { return _createdOn; }   
    }

    /// <summary>Gets the time the entity was last updated</summary>
    [System.Diagnostics.DebuggerNonUserCode]
    public System.DateTime UpdatedOn
    {
      get { return _updatedOn; }   
    }

    #endregion
  }


  [Serializable]
  [System.CodeDom.Compiler.GeneratedCode("LightSpeedModelGenerator", "1.0.0.0")]
  [System.ComponentModel.DataObject]
  public partial class Identity : Entity<int>
  {
    #region Fields
  
    private string _entityName;
    private int _customerId;

    #endregion
    
    #region Field attribute and view names
    
    /// <summary>Identifies the EntityName entity attribute.</summary>
    public const string EntityNameField = "EntityName";
    /// <summary>Identifies the CustomerId entity attribute.</summary>
    public const string CustomerIdField = "CustomerId";


    #endregion
    
    #region Relationships

    [ReverseAssociation("Identities")]
    private readonly EntityHolder<Customer> _customer = new EntityHolder<Customer>();


    #endregion
    
    #region Properties

    [System.Diagnostics.DebuggerNonUserCode]
    public Customer Customer
    {
      get { return Get(_customer); }
      set { Set(_customer, value); }
    }


    [System.Diagnostics.DebuggerNonUserCode]
    public string EntityName
    {
      get { return Get(ref _entityName, "EntityName"); }
      set { Set(ref _entityName, value, "EntityName"); }
    }

    /// <summary>Gets or sets the ID for the <see cref="Customer" /> property.</summary>
    [System.Diagnostics.DebuggerNonUserCode]
    public int CustomerId
    {
      get { return Get(ref _customerId, "CustomerId"); }
      set { Set(ref _customerId, value, "CustomerId"); }
    }

    #endregion
  }


  [Serializable]
  [System.CodeDom.Compiler.GeneratedCode("LightSpeedModelGenerator", "1.0.0.0")]
  [System.ComponentModel.DataObject]
  public partial class Role : Entity<int>
  {
    #region Fields
  
    private string _roleName;

    #endregion
    
    #region Field attribute and view names
    
    /// <summary>Identifies the RoleName entity attribute.</summary>
    public const string RoleNameField = "RoleName";


    #endregion
    
    #region Relationships

    [ReverseAssociation("Role")]
    private readonly EntityCollection<CustomerRole> _customerRoles = new EntityCollection<CustomerRole>();

    private ThroughAssociation<CustomerRole, Customer> _customers;

    #endregion
    
    #region Properties

    [System.Diagnostics.DebuggerNonUserCode]
    public EntityCollection<CustomerRole> CustomerRoles
    {
      get { return Get(_customerRoles); }
    }

    [System.Diagnostics.DebuggerNonUserCode]
    public ThroughAssociation<CustomerRole, Customer> Customers
    {
      get
      {
        if (_customers == null)
        {
          _customers = new ThroughAssociation<CustomerRole, Customer>(_customerRoles);
        }
        return Get(_customers);
      }
    }
    

    [System.Diagnostics.DebuggerNonUserCode]
    public string RoleName
    {
      get { return Get(ref _roleName, "RoleName"); }
      set { Set(ref _roleName, value, "RoleName"); }
    }

    #endregion
  }


  [Serializable]
  [System.CodeDom.Compiler.GeneratedCode("LightSpeedModelGenerator", "1.0.0.0")]
  [System.ComponentModel.DataObject]
  public partial class CustomerRole : Entity<int>
  {
    #region Fields
  
    private int _customerId;
    private int _roleId;

    #endregion
    
    #region Field attribute and view names
    
    /// <summary>Identifies the CustomerId entity attribute.</summary>
    public const string CustomerIdField = "CustomerId";
    /// <summary>Identifies the RoleId entity attribute.</summary>
    public const string RoleIdField = "RoleId";


    #endregion
    
    #region Relationships

    [ReverseAssociation("CustomerRoles")]
    private readonly EntityHolder<Customer> _customer = new EntityHolder<Customer>();
    [ReverseAssociation("CustomerRoles")]
    private readonly EntityHolder<Role> _role = new EntityHolder<Role>();


    #endregion
    
    #region Properties

    [System.Diagnostics.DebuggerNonUserCode]
    public Customer Customer
    {
      get { return Get(_customer); }
      set { Set(_customer, value); }
    }

    [System.Diagnostics.DebuggerNonUserCode]
    public Role Role
    {
      get { return Get(_role); }
      set { Set(_role, value); }
    }


    [System.Diagnostics.DebuggerNonUserCode]
    public int CustomerId
    {
      get { return Get(ref _customerId, "CustomerId"); }
      set { Set(ref _customerId, value, "CustomerId"); }
    }

    [System.Diagnostics.DebuggerNonUserCode]
    public int RoleId
    {
      get { return Get(ref _roleId, "RoleId"); }
      set { Set(ref _roleId, value, "RoleId"); }
    }

    #endregion
  }




  /// <summary>
  /// Provides a strong-typed unit of work for working with the FairlieAuthentic model.
  /// </summary>
  [System.CodeDom.Compiler.GeneratedCode("LightSpeedModelGenerator", "1.0.0.0")]
  public partial class FairlieAuthenticUnitOfWork : Mindscape.LightSpeed.UnitOfWork
  {

    public System.Linq.IQueryable<Customer> Customers
    {
      get { return this.Query<Customer>(); }
    }
    
    public System.Linq.IQueryable<Identity> Identities
    {
      get { return this.Query<Identity>(); }
    }
    
    public System.Linq.IQueryable<Role> Roles
    {
      get { return this.Query<Role>(); }
    }
    
    public System.Linq.IQueryable<CustomerRole> CustomerRoles
    {
      get { return this.Query<CustomerRole>(); }
    }
    
  }

}
