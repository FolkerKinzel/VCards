# UID
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

# RELATED

   Purpose:  To specify a relationship between another entity and the
      entity represented by this vCard.

   Value type:  A single URI.  It can also be reset to a single text
      value.  The text value can be used to specify textual information.

   Cardinality:  *

   Special notes:  The TYPE parameter MAY be used to characterize the
      related entity.  It contains a comma-separated list of values that
      are registered with IANA as described in Section 10.2.  The
      registry is pre-populated with the values defined in [xfn].  This
      document also specifies two additional values:

```
      agent:  an entity who may sometimes act on behalf of the entity
         associated with the vCard.

      emergency:  indicates an emergency contact
```

ABNF:
```
     RELATED-param = RELATED-param-uri / RELATED-param-text
     RELATED-value = URI / text
       ; Parameter and value MUST match.

     RELATED-param-uri = "VALUE=uri" / mediatype-param
     RELATED-param-text = "VALUE=text" / language-param

     RELATED-param =/ pid-param / pref-param / altid-param / type-param
                    / any-param

     type-param-related = related-type-value *("," related-type-value)
       ; type-param-related MUST NOT be used with a property other than
       ; RELATED.

     related-type-value = "contact" / "acquaintance" / "friend" / "met"
                        / "co-worker" / "colleague" / "co-resident"
                        / "neighbor" / "child" / "parent"
                        / "sibling" / "spouse" / "kin" / "muse"
                        / "crush" / "date" / "sweetheart" / "me"
                        / "agent" / "emergency"
```

Examples:
```
   RELATED;TYPE=friend:urn:uuid:f81d4fae-7dec-11d0-a765-00a0c91e6bf6
   RELATED;TYPE=contact:http://example.com/directory/jdoe.vcf
   RELATED;TYPE=co-worker;VALUE=text:Please contact my assistant Jane
    Doe for any inquiries.
```

# MEMBER

   Purpose:  To include a member in the group this vCard represents.

   Value type:  A single URI.  It MAY refer to something other than a
      vCard object.  For example, an email distribution list could
      employ the "mailto" URI scheme [RFC6068] for efficiency.

   Cardinality:  *

   Special notes:  This property MUST NOT be present unless the value of
      the KIND property is "group".

ABNF:
```
     MEMBER-param = "VALUE=uri" / pid-param / pref-param / altid-param
                  / mediatype-param / any-param
     MEMBER-value = URI
```


Examples:
```
     BEGIN:VCARD
     VERSION:4.0
     KIND:group
     FN:The Doe family
     MEMBER:urn:uuid:03a0e51f-d1aa-4385-8a53-e29025acd8af
     MEMBER:urn:uuid:b8767877-b4a1-4c70-9acc-505d3819e519
     END:VCARD
     BEGIN:VCARD
     VERSION:4.0
     FN:John Doe
     UID:urn:uuid:03a0e51f-d1aa-4385-8a53-e29025acd8af
     END:VCARD
     BEGIN:VCARD
     VERSION:4.0
     FN:Jane Doe
     UID:urn:uuid:b8767877-b4a1-4c70-9acc-505d3819e519
     END:VCARD

     BEGIN:VCARD
     VERSION:4.0
     KIND:group
     FN:Funky distribution list
     MEMBER:mailto:subscriber1@example.com
     MEMBER:xmpp:subscriber2@example.com
     MEMBER:sip:subscriber3@example.com
     MEMBER:tel:+1-418-555-5555
     END:VCARD
```




