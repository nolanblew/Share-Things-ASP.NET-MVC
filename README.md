Share-Things-ASP.NET-MVC
========================

Share Things as ASP.NET MVC Project

-------------------------------------------

===STRUCTURE===
Website files are in ShareThings/
-- Controllers are in Controllers/
-- Vies are in Views/
-- View Layout page (master page) is in Vies/Shared/_Layout (and _HeadMenu)

===How the code works===
---VIEWS---
HTML view files are .cshtml instead of .html.erb
Code with C# server-side code begin with @
Sections of code with C# are enclosed in @{ ... }
In URLs, relative file paths starting at the root directory begin with ~/

---Controller---
Lots of changes here:
Since C# is Type-based, every variable will start with a type (such as string, int, etc.)
To connect to the database, a variable "db" is the variable (so to get items, you use db.items)
  --The db variable is set at the beginning of each controller
I created a custom "filter" called [AuthorizeFilter] which will automatically redirect users to the login screen when their session expires or they're not logged in (or if Admin=true then they have to be an admin)
Authorization is used as a User and encrypted in SHA-1 before being testested against the database

--Models--
In the Models/ folder, don't really worry about any of the ShareThingsDb.* or sysdiagram.cs

==Other Stuff==
The Scripts folder is important - it contains all the javascript scripts
Content is also important as it contains all the css and images

If you have any questions feel free to ask me