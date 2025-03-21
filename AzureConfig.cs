using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StudentTermTracker
{
    public static class AzureConfig
    {
        public const string ClientId = "TODO: Get from azure app registration";
        public const string TenantId = "TODO: Get tenant id from azure ad";
        public const string StorageConnectionString = "TODO: Get storage account connection string from Az Storage account";
        public const string TableName = "Users";
    }
}
