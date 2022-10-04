# Shopping-Application
Contains two solutions - ShoppingCart.sln (client side), and ShoppingCartApplication.API.sln (server side). <br />
A RESTful full-stack marketplace app where users can list items for sale or purchase items from other vendors in C#.NET and XAML.
The application started as a console-based application that was capable of doing rudimentary CRUD operations. It had a base class Product 
and two derived classes ProductByQuantity and ProductByWeight. <br />
There also were two more classes Inventory and Cart, they provided basic CRUD operations on products.
After that, this console application was converted into a thick client using UWP, also check-out functionality was added. <br />
From there on, it was time to make this eCommerce platform web enable. I did so by implementing CRUD functionality for Inventory that persists in RAM on the server, and by
implementing functionality that allows managing the user's cart on the server. <br />
As the last step, I implemented persistence by creating the files Filebase.cs and FakeDatabase .cs (that simulated the database)
to support server-side saving through all of CRUD operations. As the result, after restarting both the client and server-side solutions
all data was in the same state it was in before the restart.
