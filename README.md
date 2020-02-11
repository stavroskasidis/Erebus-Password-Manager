# Erebus-Password-Manager

[![Greenkeeper badge](https://badges.greenkeeper.io/stavroskasidis/Erebus-Password-Manager.svg)](https://greenkeeper.io/)

An open source, password manager for personal use.

# Features:
- Web Based
- Mobile App for Android, iPhone and Windows 10 (Client or Standalone)
- Aes encryption
- Cross platform server installation (Windows - linux - mac)
- Multiple languages

# Server Installation
### Windows
#### Automatic
1. Download the installation package from here (TODO: add link).
2. Extract the package.
3. Run the "install.bat"

#### Manual
1. Download the installation package from here (TODO: add link).
2. Extract the package.
3. Install IIS (https://docs.asp.net/en/latest/publishing/iis.html#iis-configuration).
4. Install the .Net Core Windows Server Hosting (https://docs.asp.net/en/latest/publishing/iis.html#install-the-net-core-windows-server-hosting-bundle).
5. Copy the contents of the "site_files" in a folder in your IIS folder (e.g. C:\inetpub\Erebus).
6. Create a new application pool and set the CLR Version to "No Managed Code".
7. Create a new website using the application pool you created in step 6 and the physical path created in step 5. For personal use (e.g. your home pc) it is recommended to leave the hostname black and use a port other than 80 (e.g. 8080).
8. (**Optional: If you want to use the mobile client**) Open the port you used in step 7 in windows firewall.


### Linux
TODO: write

# Technical Details
- Web server written in ASP Core
- Mobile app written in Xamarin Forms
- 99% code reusability using .net standard
- Designed with SOLID principles
