## vCard 2.1:

This property specifies a value that represents a persistent, globally unique identifier associated 
with the object. The property can be used as a mechanism to relate different vCard objects. 
Some examples of valid forms of unique identifiers would include ISO 9070 formal public 
identifiers (FPI), X.500 distinguished names, machine-generated “random” numbers with a 
statistically high likelihood of being globally unique and Uniform Resource Locators (URL). If 
an URL is specified, it is suggested that the URL reference a service which will produce an 
updated version of the vCard.

This property is identified by the property name UID. This property is provided to enable a 
vCard Reader and Writer to uniquely identify either a vCard object instance or properties within
a vCard object. Valid values for this property are a unique character string. The following is an 
example of this property:

```
UID:19950401-080045-40000F192713-0052
```

Support for this property is optional for vCard Writers conforming to this specification.


## vCard 3.0:
To: ietf-mime-directory@imc.org

Subject: Registration of text/directory MIME type UID

Type name: UID

Type purpose: To specify a value that represents a globally unique
identifier corresponding to the individual or resource associated
with the vCard.

Type encoding: 8bit

Type value: A single text value.

Type special notes: The type is used to uniquely identify the object
that the vCard represents.

The type can include the type parameter "TYPE" to specify the format
of the identifier. The TYPE parameter value should be an IANA
registered identifier format. The value can also be a non-standard
format.

Type example:
```
        UID:19950401-080045-40000F192713-0052
````

```

;For name="UID"
   param        = ""
        ; No parameters allowed

   value        = text-value
```

## vCard 4.0:
**Purpose:** To specify a value that represents a globally unique
      identifier corresponding to the entity associated with the vCard.

**Value type:** A single URI value.  It MAY also be reset to free-form
      text.

**Cardinality:**  *1

**Special notes:**  This property is used to uniquely identify the object
      that the vCard represents.  The "uuid" URN namespace defined in
      [RFC4122] is particularly well suited to this task, but other URI
      schemes MAY be used.  Free-form text MAY also be used.

**ABNF:**
```
     UID-param = UID-uri-param / UID-text-param
     UID-value = UID-uri-value / UID-text-value
       ; Value and parameter MUST match.

     UID-uri-param = "VALUE=uri"
     UID-uri-value = URI

     UID-text-param = "VALUE=text"
     UID-text-value = text

     UID-param =/ any-param
```

**Example:**
```
           UID:urn:uuid:f81d4fae-7dec-11d0-a765-00a0c91e6bf6
```


