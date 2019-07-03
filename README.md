# Introduction 
TODO: Give a short introduction of your project. Let this section explain the objectives or the motivation behind this project. 

# Getting Started
TODO: Guide users through getting your code up and running on their own system. In this section you can talk about:
1.	Installation process
2.	Software dependencies
3.	Latest releases
4.	API references

# Build and Test
TODO: Describe and show how to build your code and run the tests. 

# Contribute
TODO: Explain how other users and developers can contribute to make your code better. 

If you want to learn more about creating good readme files then refer the following [guidelines](https://www.visualstudio.com/en-us/docs/git/create-a-readme). You can also seek inspiration from the below readme files:
- [ASP.NET Core](https://github.com/aspnet/Home)
- [Visual Studio Code](https://github.com/Microsoft/vscode)
- [Chakra Core](https://github.com/Microsoft/ChakraCore)


# DC-ILR-1920-ReferenceDataService

# Command used to Generate Repo Contents from another repo

## CD to folder to create clone repo in

```
mkdir CloneRepo
cd CloneRepo
git clone --bare https://sfa-gov-uk@dev.azure.com/sfa-gov-uk/DCT/_git/DC-ILR-1819-ReferenceDataService
cd DC-ILR-1819-ReferenceDataService.git
git push --mirror https://sfa-gov-uk@dev.azure.com/sfa-gov-uk/DCT/_git/DC-ILR-1920-ReferenceDataService
cd.. 
rmdir /s DC-ILR-1819-ReferenceDataService.git
```