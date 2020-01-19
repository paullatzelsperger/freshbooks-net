using System;
using System.Collections.Generic;

namespace FreshbooksApiClient.Contracts
{
public class Profession
{
    public int id { get; set; }
    public string title { get; set; }
    public string company { get; set; }
    public object designation { get; set; }
}

public class Profile
{
    public bool setup_complete { get; set; }
    public string first_name { get; set; }
    public string last_name { get; set; }
    public object phone_number { get; set; }
    public object address { get; set; }
    public List<Profession> professions { get; set; }
}

public class PhoneNumber
{
    public string title { get; set; }
    public object phone_number { get; set; }
}

public class Profession2
{
    public int id { get; set; }
    public string title { get; set; }
    public string company { get; set; }
    public object designation { get; set; }
}

public class Links
{
    public string me { get; set; }
    public string roles { get; set; }
}

public class Group
{
    public int id { get; set; }
    public int group_id { get; set; }
    public string role { get; set; }
    public int identity_id { get; set; }
    public string first_name { get; set; }
    public string last_name { get; set; }
    public string email { get; set; }
    public string company { get; set; }
    public int business_id { get; set; }
    public bool active { get; set; }
}

public class SubscriptionStatuses
{
    public string zDmNq { get; set; }
    public string e6Wmk { get; set; }
}

public class Integrations
{
}

public class Address
{
    public int id { get; set; }
    public string street { get; set; }
    public string city { get; set; }
    public string province { get; set; }
    public string country { get; set; }
    public string postal_code { get; set; }
}

public class Business
{
    public int id { get; set; }
    public string name { get; set; }
    public string account_id { get; set; }
    public Address address { get; set; }
    public object phone_number { get; set; }
    public List<Client> business_clients { get; set; }
}

public class Client
{
    public long? id { get; set; }
    public long business_id{ get; set; }
    public string account_id{ get; set; }
}

public class BusinessMembership
{
    public int id { get; set; }
    public string role { get; set; }
    public Business business { get; set; }
}

public class Links2
{
    public string destroy { get; set; }
}

public class Role
{
    public int id { get; set; }
    public string role { get; set; }
    public int systemid { get; set; }
    public int userid { get; set; }
    public DateTime created_at { get; set; }
    public Links2 links { get; set; }
    public string accountid { get; set; }
}

public class Response
{
    public int id { get; set; }
    public Profile profile { get; set; }
    public string first_name { get; set; }
    public string last_name { get; set; }
    public string email { get; set; }
    public DateTime confirmed_at { get; set; }
    public DateTime created_at { get; set; }
    public object unconfirmed_email { get; set; }
    public bool setup_complete { get; set; }
    public List<PhoneNumber> phone_numbers { get; set; }
    public List<object> addresses { get; set; }
    public Profession2 profession { get; set; }
    public Links links { get; set; }
    public object permissions { get; set; }
    public List<Group> groups { get; set; }
    public SubscriptionStatuses subscription_statuses { get; set; }
    public Integrations integrations { get; set; }
    public List<BusinessMembership> business_memberships { get; set; }
    public List<Role> roles { get; set; }
}

public class IdentityInfo
{
    public Response response { get; set; }
}
}