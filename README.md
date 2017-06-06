# Some test boilerplate with dotnet core

*Still a work in progress!*

## Docker

I run postgres and mailcatcher through docker, but you can configure these on your local machine if you want.
```
docker-compose build
docker-compose up
```

Then in the Data.PostgreSQL project to seed the schema
```
dotnet ef database update
```

Navigate to localhost:1080 to see the mailcatcher GUI.

## pgAdmin4

Useful GUI tool for postgreSQL manipulation.

```
docker run --rm -p 5050:5050 thajeztah/pgadmin4
```

## Authentication Server

In a separate terminal you can run the Auth project for a simple auth provider
```
dotnet restore && dotnet run
```

Example curl login script:
```
curl -X POST \
  http://localhost:5050/connect/token \
  -H 'cache-control: no-cache' \
  -H 'content-type: application/x-www-form-urlencoded' \
  -H 'postman-token: fd42135a-aa5c-e2f6-d07f-48be291d16ed' \
  -d 'grant_type=password&scope=customAPI.write&client_id=oauth2client&client_secret=notasecret&username=john.doe%40fake.com&password=password123'
```

## Application Server

The actual app is in the Api folder.
```
dotnet restore && dotnet run
```

And here are some test endpoints I had created:

### Basic Test
```
curl -X GET \
  http://localhost:5080/api/test/test \
  -H 'accept-language: en-US' \
  -H 'cache-control: no-cache' \
  -H 'postman-token: aadd4909-6a66-62f1-19cb-6b0ca31e68ed'
```

### Protected Test
```
curl -X GET \
  http://localhost:5080/api/test/testprotected \
  -H 'accept-language: en-US' \
  -H 'authorization: Bearer eyJhbGciOiJIUzUxMiIsInR5cCI6IkpXVCJ9.eyJuYmYiOjE0OTU1OTkwMDEsImV4cCI6MTQ5NTYwMjYwMSwiaXNzIjoiaHR0cDovL2xvY2FsaG9zdDo1MDUwIiwiYXVkIjpbImh0dHA6Ly9sb2NhbGhvc3Q6NTA1MC9yZXNvdXJjZXMiLCJjdXN0b21BUEkiXSwiY2xpZW50X2lkIjoib2F1dGgyY2xpZW50Iiwic3ViIjoiU1VCSkVDVElEIiwiYXV0aF90aW1lIjoxNDk1NTk5MDAxLCJpZHAiOiJsb2NhbCIsImVtYWlsIjoiam9obi5kb2VAZmFrZS5jb20iLCJyb2xlIjoiYWRtaW4iLCJwcmVmZXJyZWRfdXNlcm5hbWUiOiJqb2huLmRvZUBmYWtlLmNvbSIsImxvY2FsZSI6ImVuLUNBIiwic2NvcGUiOlsiY3VzdG9tQVBJLndyaXRlIl0sImFtciI6WyJwd2QiXX0.cyXNrU17hzacDesUXBOVNp7-la1JLcKYPErP1KiFQIxvD2i2e6GAQ6otc5TDuutO3JZdjT1jLM6JKO-hSlYBBQ' \
  -H 'cache-control: no-cache' \
  -H 'postman-token: 8ef8cbac-1103-7aed-df68-7b2e3db5d9d9'
```

### Exception Test
```
curl -X GET \
  http://localhost:5080/api/test/testexception \
  -H 'cache-control: no-cache' \
  -H 'postman-token: baf037e4-580e-ce9c-3634-478f765d4963'
```

### Culture File Test
```
curl -X GET \
  http://localhost:5080/api/culture/translationJson/en \
  -H 'cache-control: no-cache' \
  -H 'postman-token: c6a4aa43-2498-3df7-652a-f6ed62cbdfde'
```

### Email Test
```
curl -X GET \
  http://localhost:5080/api/test/emailtest \
  -H 'cache-control: no-cache' \
  -H 'postman-token: 989f50f3-cf21-529e-ae69-02104ed31fa2'
```
