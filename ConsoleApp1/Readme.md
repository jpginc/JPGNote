install dotnet whatever (https://dotnet.microsoft.com/download/dotnet-framework/net472?utm_source=getdotnet&utm_medium=referral)
install mono 64 bit (https://www.mono-project.com/download/stable/)
install msy2 https://www.msys2.org/
update pacman -Syu
setup gtk https://www.gtk.org/download/windows.php

` pacman -S mingw-w64-x86_64-gtk3 `

 i don't think you need this 
`  pacman -S mingw-w64-x86_64-glade `

you might need to change the ConsoleApp1.csproj to look like this

```
<Project Sdk="Microsoft.NET.Sdk">
  <PropertyGroup>
    <OutputType>Exe</OutputType>
    <TargetFramework>netcoreapp2.0</TargetFramework>
    <Platforms>AnyCPU;x64;x86</Platforms>
    <ApplicationIcon/>
    <StartupObject/>
  </PropertyGroup>
  <PropertyGroup Condition="'$(Configuration)|$(Platform)'=='Release|AnyCPU'">
    <GenerateSerializationAssemblies>Auto</GenerateSerializationAssemblies>
  </PropertyGroup>
  <PropertyGroup>
    <RootNamespace>ConsoleApp1</RootNamespace>
  </PropertyGroup>
  <ItemGroup>
    <Content Include="*.cs"/>
    <Content Include="Actions\*.cs"/>
    <Content Include="Gui\*.cs"/>
    <Content Include="Managers\*.cs"/>
    <Content Include="Menus\*.cs"/>
    <Content Include="Settings\*.cs"/>
    <Content Include="Settings\*.cs"/>
  </ItemGroup>
  <ItemGroup>
    <PackageReference Include="GtkSharp" Version="*"/>
  </ItemGroup>
</Project>
```

then you can create a shortcut to run it something like 
`"C:\Program Files\dotnet\dotnet.exe" run --project "c:\Users\Administrator\Desktop\JPGNote\ConsoleApp1\ConsoleApp1.csproj"`

The API for the parsing code stuff is in CommandManager.cs:
add a target
Target\r\n
*value*\r\n

add a port
Port\r\n
*value*\r\n
Done\r\n

add a port with a tag
Port\r\n
*value*\r\n
TAG:\r\n
*value*\r\n
TAG:\r\n
*value*\r\n
...
Done\r\n


