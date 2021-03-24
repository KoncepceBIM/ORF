﻿//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by a tool.
//     Runtime Version:4.0.30319.42000
//
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

using System.Xml.Serialization;

// 
// This source code was auto-generated by xsd, Version=4.8.3928.0.
// 


/// <remarks/>
[System.CodeDom.Compiler.GeneratedCodeAttribute("xsd", "4.8.3928.0")]
[System.SerializableAttribute()]
[System.Diagnostics.DebuggerStepThroughAttribute()]
[System.ComponentModel.DesignerCategoryAttribute("code")]
[System.Xml.Serialization.XmlTypeAttribute(Namespace="http://tempuri.org/Units.xsd")]
[System.Xml.Serialization.XmlRootAttribute("Units", Namespace="http://tempuri.org/Units.xsd", IsNullable=false)]
public partial class TUnitList {
    
    private TUnit[] unitField;
    
    /// <remarks/>
    [System.Xml.Serialization.XmlElementAttribute("Unit")]
    public TUnit[] Unit {
        get {
            return this.unitField;
        }
        set {
            this.unitField = value;
        }
    }
}

/// <remarks/>
[System.Xml.Serialization.XmlIncludeAttribute(typeof(TMonetaryUnit))]
[System.Xml.Serialization.XmlIncludeAttribute(typeof(TContextDependentUnit))]
[System.Xml.Serialization.XmlIncludeAttribute(typeof(TConversionUnit))]
[System.Xml.Serialization.XmlIncludeAttribute(typeof(TDerivedUnit))]
[System.Xml.Serialization.XmlIncludeAttribute(typeof(TSIUnit))]
[System.CodeDom.Compiler.GeneratedCodeAttribute("xsd", "4.8.3928.0")]
[System.SerializableAttribute()]
[System.Diagnostics.DebuggerStepThroughAttribute()]
[System.ComponentModel.DesignerCategoryAttribute("code")]
[System.Xml.Serialization.XmlTypeAttribute(Namespace="http://tempuri.org/Units.xsd")]
public abstract partial class TUnit {
}

/// <remarks/>
[System.CodeDom.Compiler.GeneratedCodeAttribute("xsd", "4.8.3928.0")]
[System.SerializableAttribute()]
[System.Diagnostics.DebuggerStepThroughAttribute()]
[System.ComponentModel.DesignerCategoryAttribute("code")]
[System.Xml.Serialization.XmlTypeAttribute(Namespace="http://tempuri.org/Units.xsd")]
public partial class TMonetaryUnit : TUnit {
    
    private string currencyField;
    
    /// <remarks/>
    [System.Xml.Serialization.XmlAttributeAttribute()]
    public string Currency {
        get {
            return this.currencyField;
        }
        set {
            this.currencyField = value;
        }
    }
}

/// <remarks/>
[System.CodeDom.Compiler.GeneratedCodeAttribute("xsd", "4.8.3928.0")]
[System.SerializableAttribute()]
[System.Diagnostics.DebuggerStepThroughAttribute()]
[System.ComponentModel.DesignerCategoryAttribute("code")]
[System.Xml.Serialization.XmlTypeAttribute(Namespace="http://tempuri.org/Units.xsd")]
public partial class TContextDependentUnit : TUnit {
    
    private string nameField;
    
    private string symbolField;
    
    /// <remarks/>
    [System.Xml.Serialization.XmlAttributeAttribute()]
    public string Name {
        get {
            return this.nameField;
        }
        set {
            this.nameField = value;
        }
    }
    
    /// <remarks/>
    [System.Xml.Serialization.XmlAttributeAttribute()]
    public string Symbol {
        get {
            return this.symbolField;
        }
        set {
            this.symbolField = value;
        }
    }
}

/// <remarks/>
[System.CodeDom.Compiler.GeneratedCodeAttribute("xsd", "4.8.3928.0")]
[System.SerializableAttribute()]
[System.Diagnostics.DebuggerStepThroughAttribute()]
[System.ComponentModel.DesignerCategoryAttribute("code")]
[System.Xml.Serialization.XmlTypeAttribute(Namespace="http://tempuri.org/Units.xsd")]
public partial class TConversionUnit : TUnit {
    
    private TSIUnit baseUnitField;
    
    private string nameField;
    
    private string symbolField;
    
    private double scaleField;
    
    private double offsetField;
    
    public TConversionUnit() {
        this.scaleField = 1D;
        this.offsetField = 0D;
    }
    
    /// <remarks/>
    public TSIUnit BaseUnit {
        get {
            return this.baseUnitField;
        }
        set {
            this.baseUnitField = value;
        }
    }
    
    /// <remarks/>
    [System.Xml.Serialization.XmlAttributeAttribute()]
    public string Name {
        get {
            return this.nameField;
        }
        set {
            this.nameField = value;
        }
    }
    
    /// <remarks/>
    [System.Xml.Serialization.XmlAttributeAttribute()]
    public string Symbol {
        get {
            return this.symbolField;
        }
        set {
            this.symbolField = value;
        }
    }
    
    /// <remarks/>
    [System.Xml.Serialization.XmlAttributeAttribute()]
    [System.ComponentModel.DefaultValueAttribute(1D)]
    public double Scale {
        get {
            return this.scaleField;
        }
        set {
            this.scaleField = value;
        }
    }
    
    /// <remarks/>
    [System.Xml.Serialization.XmlAttributeAttribute()]
    [System.ComponentModel.DefaultValueAttribute(0D)]
    public double Offset {
        get {
            return this.offsetField;
        }
        set {
            this.offsetField = value;
        }
    }
}

/// <remarks/>
[System.CodeDom.Compiler.GeneratedCodeAttribute("xsd", "4.8.3928.0")]
[System.SerializableAttribute()]
[System.Diagnostics.DebuggerStepThroughAttribute()]
[System.ComponentModel.DesignerCategoryAttribute("code")]
[System.Xml.Serialization.XmlTypeAttribute(Namespace="http://tempuri.org/Units.xsd")]
public partial class TSIUnit : TUnit {
    
    private SIUnitName nameField;
    
    private SIUnitPrefix prefixField;
    
    private bool prefixFieldSpecified;
    
    /// <remarks/>
    [System.Xml.Serialization.XmlAttributeAttribute()]
    public SIUnitName Name {
        get {
            return this.nameField;
        }
        set {
            this.nameField = value;
        }
    }
    
    /// <remarks/>
    [System.Xml.Serialization.XmlAttributeAttribute()]
    public SIUnitPrefix Prefix {
        get {
            return this.prefixField;
        }
        set {
            this.prefixField = value;
        }
    }
    
    /// <remarks/>
    [System.Xml.Serialization.XmlIgnoreAttribute()]
    public bool PrefixSpecified {
        get {
            return this.prefixFieldSpecified;
        }
        set {
            this.prefixFieldSpecified = value;
        }
    }
}

/// <remarks/>
[System.CodeDom.Compiler.GeneratedCodeAttribute("xsd", "4.8.3928.0")]
[System.SerializableAttribute()]
[System.Xml.Serialization.XmlTypeAttribute(Namespace="http://tempuri.org/Units.xsd")]
public enum SIUnitName {
    
    /// <remarks/>
    metre,
    
    /// <remarks/>
    gram,
    
    /// <remarks/>
    second,
    
    /// <remarks/>
    ampere,
    
    /// <remarks/>
    kelvin,
    
    /// <remarks/>
    mole,
    
    /// <remarks/>
    candela,
    
    /// <remarks/>
    radian,
    
    /// <remarks/>
    steradian,
    
    /// <remarks/>
    hertz,
    
    /// <remarks/>
    newton,
    
    /// <remarks/>
    pascal,
    
    /// <remarks/>
    joule,
    
    /// <remarks/>
    watt,
    
    /// <remarks/>
    coulomb,
    
    /// <remarks/>
    volt,
    
    /// <remarks/>
    farad,
    
    /// <remarks/>
    ohm,
    
    /// <remarks/>
    siemens,
    
    /// <remarks/>
    weber,
    
    /// <remarks/>
    tesla,
    
    /// <remarks/>
    henry,
    
    /// <remarks/>
    degree_Celsius,
    
    /// <remarks/>
    lumen,
    
    /// <remarks/>
    lux,
    
    /// <remarks/>
    becquerel,
    
    /// <remarks/>
    gray,
    
    /// <remarks/>
    sievert,
    
    /// <remarks/>
    katal,
}

/// <remarks/>
[System.CodeDom.Compiler.GeneratedCodeAttribute("xsd", "4.8.3928.0")]
[System.SerializableAttribute()]
[System.Xml.Serialization.XmlTypeAttribute(Namespace="http://tempuri.org/Units.xsd")]
public enum SIUnitPrefix {
    
    /// <remarks/>
    yotta,
    
    /// <remarks/>
    zetta,
    
    /// <remarks/>
    exa,
    
    /// <remarks/>
    peta,
    
    /// <remarks/>
    tera,
    
    /// <remarks/>
    giga,
    
    /// <remarks/>
    mega,
    
    /// <remarks/>
    kilo,
    
    /// <remarks/>
    hecto,
    
    /// <remarks/>
    deca,
    
    /// <remarks/>
    deci,
    
    /// <remarks/>
    centi,
    
    /// <remarks/>
    milli,
    
    /// <remarks/>
    micro,
    
    /// <remarks/>
    nano,
    
    /// <remarks/>
    pico,
    
    /// <remarks/>
    femto,
    
    /// <remarks/>
    atto,
    
    /// <remarks/>
    zepto,
    
    /// <remarks/>
    yocto,
}

/// <remarks/>
[System.CodeDom.Compiler.GeneratedCodeAttribute("xsd", "4.8.3928.0")]
[System.SerializableAttribute()]
[System.Diagnostics.DebuggerStepThroughAttribute()]
[System.ComponentModel.DesignerCategoryAttribute("code")]
[System.Xml.Serialization.XmlTypeAttribute(Namespace="http://tempuri.org/Units.xsd")]
public partial class TDerivedUnit : TUnit {
    
    private TDerivedUnitComponent[] componentField;
    
    /// <remarks/>
    [System.Xml.Serialization.XmlElementAttribute("Component")]
    public TDerivedUnitComponent[] Component {
        get {
            return this.componentField;
        }
        set {
            this.componentField = value;
        }
    }
}

/// <remarks/>
[System.CodeDom.Compiler.GeneratedCodeAttribute("xsd", "4.8.3928.0")]
[System.SerializableAttribute()]
[System.Diagnostics.DebuggerStepThroughAttribute()]
[System.ComponentModel.DesignerCategoryAttribute("code")]
[System.Xml.Serialization.XmlTypeAttribute(AnonymousType=true, Namespace="http://tempuri.org/Units.xsd")]
public partial class TDerivedUnitComponent {
    
    private TUnit unitField;
    
    private int exponentField;
    
    /// <remarks/>
    public TUnit Unit {
        get {
            return this.unitField;
        }
        set {
            this.unitField = value;
        }
    }
    
    /// <remarks/>
    [System.Xml.Serialization.XmlAttributeAttribute()]
    public int Exponent {
        get {
            return this.exponentField;
        }
        set {
            this.exponentField = value;
        }
    }
}
