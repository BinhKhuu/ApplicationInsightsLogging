# Local Settings Setup

Create enviroment variable APPLICATIONINSIGHTS_CONNECTION_STRING to target application insights.

# Blazor insights set up

1. create cookie ai_connString to enable blazor insights logging.

``` js

var cookie = document.cookie;
// replace Conn Str
var connStr = encodeURIComponent("appInsightsConnStr")
var ai_connString = `ai_connString=${connStr}`;

document.cookie = `${ai_connString}; ${cookie}`;

```

# Function set up

## db name: ApplicationInsightsLogger

## local.settings.json
``` json
  "IsEncrypted": false,
  "Values": {
    "AzureWebJobsStorage": "UseDevelopmentStorage=true",
    "FUNCTIONS_WORKER_RUNTIME": "dotnet",
    "APPLICATIONINSIGHTS_CONNECTION_STRING": "",
    "SqlServerConnectionString": "",
    "TelementryConnectionString": ""
  },
  "Host": {
    "LocalHttpPort": 7189,
    "CORS": "*"
  }

```

### Creating db for the first time
1. set .api project as start up
1. package manager console target .infrastructure project
1. set connection strings in local.settings and ApplicationInsightsLoggerContextFactory
1. run ```Update-Database``` note you may need to create db named ApplicationInsightsLogger first
1. 

### Creating db from scratch
1. delete migrations
1. set .api project as start up
1. package manager console target .infrastructure project
1. set connection strings in local.settings and ApplicationInsightsLoggerContextFactory
1. run ``` Add-Migration <<MyDBMigration>>``` and ```Update-Database```


