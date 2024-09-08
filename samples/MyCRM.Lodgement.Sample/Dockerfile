FROM mcr.microsoft.com/dotnet/sdk:8.0-bookworm-slim as build
WORKDIR /app
EXPOSE 44362

COPY . .

WORKDIR "/app/samples/MyCRM.Lodgement.Sample/"

RUN --mount=type=secret,id=NUGET_LOGIN_TOKEN <<EOF
  nugetLoginToken=$(cat /run/secrets/NUGET_LOGIN_TOKEN)
  if [ ! -z "$nugetLoginToken" ]; then   
    dotnet nuget remove source lmgithubnuget
    dotnet nuget add source https://nuget.pkg.github.com/loanmarket/index.json \
      -u license.admin@loanmarket.com.au -p "${nugetLoginToken}" -n lmgithubnuget \
      --store-password-in-clear-text
  fi
EOF

RUN dotnet publish \
    --configuration Release \
    --output "/app/publish"

FROM mcr.microsoft.com/dotnet/aspnet:8.0-bookworm-slim AS final
WORKDIR /app
COPY --from=build /app/publish .

LABEL org.opencontainers.image.source=https://github.com/loanmarket/mycrm-lodgement-sample

ENTRYPOINT ["dotnet", "MyCRM.Lodgement.Sample.dll"]
