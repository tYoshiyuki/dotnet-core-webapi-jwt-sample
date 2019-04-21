# dotnet-core-webapi-jwt-sample
ASP.NET Core の JWT で認証をする Web API のサンプル

## Feature
- .NET Core 2.2
- ASP.NET Core
- ASP.NET Core Identity
- JWT

## Usage
- パッケージマネージャより Update-Database
- デバッグ実行 > swagger-ui > /Account/CreateUser より任意のユーザを作成
- /Account/Login で JWTトークン を取得
- swagger-ui > Authorize より bearer {取得したJWTトークンの値} を入力
- api/Values が取得できれば認証成功

## Note
- /Account/Login で JWTトークン を発行します。
- appsettings.json の JwtSetting の内容を適宜書き換えてください。 
- Googleアカウントによるログインも実装しています。
    - Startup.cs の下記コードを修正後、/Account/SignInWithGoogle にアクセスしてください。
```C#
            }).AddGoogle("Google", options =>
            {
                    options.ClientId = "xxx";
                    options.ClientSecret = "xxx";
            });
```

- クライアントID, クライアントシークレットの取得方法は、下記を参照し適宜取得してください。
- https://docs.microsoft.com/ja-jp/aspnet/core/security/authentication/social/index?view=aspnetcore-2.2&tabs=visual-studio
