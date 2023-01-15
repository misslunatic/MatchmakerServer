<div align="center">

# Matchmaker-API - Server

  ![Linux](https://img.shields.io/badge/Linux-FCC624?style=for-the-badge&logo=linux&logoColor=black)
  ![Windows](https://img.shields.io/badge/Windows-0078D6?style=for-the-badge&logo=windows&logoColor=white)

![GitHub forks](https://img.shields.io/github/forks/Techiesplash/Matchmaker-API-Server)
![GitHub repo size](https://img.shields.io/github/repo-size/Techiesplash/Matchmaker-API-Server)
![GitHub all releases](https://img.shields.io/github/downloads/Techiesplash/Matchmaker-API-Server/total)
![GitHub issues](https://img.shields.io/github/issues/Techiesplash/Matchmaker-API-Server)

![GitHub](https://img.shields.io/github/license/Techiesplash/Matchmaker-API-Server)
![GitHub release (latest by date)](https://img.shields.io/github/v/release/Techiesplash/Matchmaker-API-Server)
  
  
<h2>Introduction</h2>
This is a project for implementing a Matchmaker API into Unity3D.
<br />
It can be expanded with custom packets as needed.
<br />
<br />

It depends on another project to be used in Unity. https://github.com/Techiesplash/Matchmaker-API-Client-Unity3d
<br />

![UVS Preview](./Images/preview.png)

## Installation
Ensure you have the latest .NET SDK and Runtime (7.0) installed.

First grab the latest release, and extract it to a directory.

Open that directory in a C# editor such as JetBrains Rider, Visual Studio or Visual Studio Code.

## Usage
First edit ```Matchmaker/Config.cs```.

The following settings need to match on both the client and the matchmaker server:

- Set ```GameId``` to the name of your game. 
- Set ```GameVersion``` to the latest version number of your game. Remember to update this as you update your game.

The rest can be changed at your discretion.

Now build and run, and it should be done.

## Authors

* **Tom Weiland** - *Initial work (TCP/UDP interface)* - [Tom Weiland](https://github.com/tom-weiland)
* **Techiesplash** - *Matchmaker API* - [Techiesplash](https://github.com/Techiesplash)

This project is built upon MIT-Licensed code by Tom Weiland meant for a tutorial series.
Please check out his work: https://github.com/tom-weiland/tcp-udp-networking

See also the list of [contributors](https://github.com/Techiesplash/ParentalControls/contributors) who participated in this project.

## License

This project is licensed under the MIT License - see the [LICENSE.md](LICENSE.md) file for details
