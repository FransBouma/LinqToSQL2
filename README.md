Linq To SQL2
=============

Official Linq to SQL fork. A complete ORM which is backwards compatible with Linq to SQL but with new features. Please see the [Wiki](https://github.com/FransBouma/LinqToSQL2/wiki) for further information about the progress and design decisions made in this project. For the roadmap/features which are planned, please see the [issues section](https://github.com/FransBouma/LinqToSQL2/issues) for the work items. 

There's no official release yet, as this project has just been started. After every new feature successfully added to the codebase, a new version will be released on Nuget. 

## Does the code in 'Trunk' compile?

(No CI system setup (yet), so we'll have to do with the manually written elements below)

This is the case: YES
Will the tests run: NO.

Reason: A massive refactoring is currently under way and it's not possible to commit a compiling code base at all times during this massive refactoring. As soon as the code has passed this massive refactoring (which is related to issue #6), we'll update this section.

## Linq to SQL and this project

This project is an official fork from Linq to SQL from the [.NET reference sourcecode](https://github.com/Microsoft/referencesource). As the reference source for .NET doesn't come in compilable form, the resource files for the error strings have been reverse engineered from the official System.Data.Linq assembly.

This project strives to stay 100% backwards compatible with Linq to SQL's query API, so your original Linq to SQL projects will just work with Linq to SQL 2, unless stated otherwise.

### License

The original Linq to SQL code is (c) Microsoft Corporation (see License.txt). Additional code added is (c) by the contributors and is marked as such in the code files. 

### Designer support

This project will offer designer support through [LLBLGen Pro](http://www.llblgen.com)

