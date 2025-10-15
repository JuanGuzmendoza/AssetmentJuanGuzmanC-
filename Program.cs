using Hospital.Menus;
using Hospital.Data;
/// <summary>
/// This is the main entry point for the application.
/// </summary>

        await DataInitializer.InitializeAsync(); // 🔹 Carga todos los diccionarios
        await Login.ShowAsync();                // 🔹 Luego muestra el login
 