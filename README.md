Linq To SQL2
=============

Official Linq to SQL fork. A complete ORM which is backwards compatible with Linq to SQL but with new features. Please see the [Wiki](https://github.com/FransBouma/LinqToSQL2/wiki) for further information about the progress and design decisions made in this project. For the roadmap/features which are planned, please see the [issues section](https://github.com/FransBouma/LinqToSQL2/issues) for the work items. 

There's no official release yet, as this project has just been started. After every new feature successfully added to the codebase, a new version will be released on Nuget. 

## Linq to SQL and this project

This project is an official fork from Linq to SQL from the [.NET reference sourcecode](https://github.com/Microsoft/referencesource). As the reference source for .NET doesn't come in compilable form, the generated resource files have been added from the official System.Data.Linq assembly. When Microsoft ports Linq to SQL to vNext these files will be replaced with the official resource files from that project. 

This project strives to stay 100% backwards compatible with Linq to SQL's query API, so your original Linq to SQL projects will just work with Linq to DB, unless stated otherwise.

### License

The original Linq to SQL code is (c) Microsoft Corporation (see License.txt). Additional code added is (c) of the contributors and is marked as such in the code files. 

### Designer support

This project will offer designer support through [LLBLGen Pro](http://www.llblgen.com)

