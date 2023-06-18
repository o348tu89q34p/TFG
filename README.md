# Continguts
Branca corresponent al lliurable parcial produït a la PAC 4.

# Requeriments
.NET 6.0 o posterior

# Instruccions per compilar i executar el joc
## Comandes
cd Client
dotnet build -c Release
Xcopy ..\Assets bin\Release\Assets\ /E
bin\Release\net6.0\Client.exe

Alternativament també es pot moure la carpeta sense fer la còpia:
cd Client
dotnet build -c Release
move ..\Assets bin\Release
bin\Release\net6.0\Client.exe
