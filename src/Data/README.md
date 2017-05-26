dotnet ef --project . --startup-project ../Api/ ...command
dotnet ef --project . --startup-project ../Api/ migrations add initial
dotnet ef --project . --startup-project ../Api/ database update
