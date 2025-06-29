# 🧾 Sistema de Gestión de Ventas - SIMCOV

Este proyecto es una aplicación de escritorio desarrollada en **C# con Windows Forms** y **SQL Server**, destinada a la gestión de ventas, productos, clientes y reportes en tiempo real. El sistema está pensado para pequeños y medianos comercios que requieren una solución rápida, sencilla y eficiente para controlar sus operaciones.

---

## 📌 Características principales

- 🔐 **Login con control de acceso por roles** (administrador / empleado).
- 📦 **Módulo de inventario**: registrar, actualizar productos, alertas de stock mínimo.
- 🧾 **Módulo de ventas**: registro de ventas, cálculo de cambio, anulación de ventas.
- 👥 **Gestión de clientes y usuarios**.
- 📊 **Módulo de reportes**: ventas diarias, productos más vendidos, exportación a Excel.
- 🔐 **Cambio de contraseña** con validación de usuario.
- 💾 Conexión directa a **SQL Server**.

---

## 🛠️ Tecnologías utilizadas

- **Lenguaje**: C#
- **Framework**: .NET Framework / Windows Forms
- **Base de datos**: SQL Server
- **ORM**: SQL puro (ADO.NET)
- **IDE**: Visual Studio 2022

---

## 📁 Estructura del Proyecto
simcov/
├── bin/
├── obj/
├── clases/
│ ├── conexion_simcov.cs
│ ├── SesionUsuario.cs
│ └── seguridad.cs
│ └── clientes.cs
│ └── inventario.cs
│ └── productos.cs
│ └── ventas.cs
├── Form1.cs (Login)
├── Form2.cs (Menú principal)
├── Form3.cs (Ventas)
├── Form4.cs (Inventario)
├── Form5.cs (Clientes)
├── Form6.cs (Reportes)
├── Form7.cs (Validación de usuario)
├── Form8.cs (Ventas registradas y anulación)
├── Form9.cs (Usuarios)
├── Form10.cs (Cambiar contraseña)
├── simcov.sql (script de base de datos)
└── README.md

---

## 🧪 Requisitos para ejecución

- Tener instalado **Visual Studio 2022 o superior**
- Tener instalado **SQL Server** (preferiblemente con SQL Server Management Studio)
- Actualizar la cadena de conexión en `conexion_simcov.cs` con los datos de tu servidor

---

## 🔄 Crear la base de datos

1. Abrir SQL Server Management Studio.
2. Crear una nueva base de datos (puede llamarse `simcov`).
3. Esquema de base de datos:

---

## Estructura de tablas
--sql

CREATE TABLE Productos (
    id_producto INT PRIMARY KEY IDENTITY(1,1),
    nombre VARCHAR(100) NOT NULL,
    precio DECIMAL(10,2) NOT NULL CHECK (precio > 0),
    costo DECIMAL(10,2) NULL, 
    stock INT NOT NULL DEFAULT 0,
    stock_minimo INT NULL, 
    fecha_creacion DATETIME DEFAULT GETDATE()
);
CREATE TABLE Clientes (
    id_cliente INT PRIMARY KEY IDENTITY(1,1),
    nombre VARCHAR(100) NOT NULL,
    telefono VARCHAR(20) NULL,
    direccion VARCHAR(200) NULL,
    fecha_registro DATETIME DEFAULT GETDATE()
);
CREATE TABLE Ventas (
    id_venta INT PRIMARY KEY IDENTITY(1,1),
    id_cliente INT NULL, 
    fecha DATETIME NOT NULL DEFAULT GETDATE(),
    total DECIMAL(12,2) NOT NULL,
    id_usuario INT NOT NULL,
    anulada BIT DEFAULT 0,
    FOREIGN KEY (id_cliente) REFERENCES Clientes(id_cliente),
    FOREIGN KEY (id_usuario) REFERENCES Usuarios(id_usuario)
);
CREATE TABLE DetalleVenta (
    id_detalle INT PRIMARY KEY IDENTITY(1,1),
    id_venta INT NULL,
    id_producto INT NULL,
    cantidad INT NOT NULL CHECK (cantidad > 0),
    precio_unitario DECIMAL(10,2) NOT NULL,  
    subtotal DECIMAL(12,2) NOT NULL,
    FOREIGN KEY (id_venta) REFERENCES Ventas(id_venta) ON DELETE SET NULL,
    FOREIGN KEY (id_producto) REFERENCES Productos(id_producto) ON DELETE SET NULL
);
CREATE TABLE Usuarios (
    id_usuario INT PRIMARY KEY IDENTITY(1,1),
    nombre_usuario VARCHAR(50) NOT NULL UNIQUE,
    contraseña VARCHAR(100) NOT NULL, 
    rol VARCHAR(20) NOT NULL CHECK (rol IN ('administrador', 'empleado')),
    fecha_creacion DATETIME DEFAULT GETDATE()
);

---

4. Realizar una primera inserción para generar un usuario con el que ingresara **con contraseña hasheada**. Ejemplo:
   ```bash
    INSERT INTO Usuarios (nombre_usuario, contraseña, rol)
    VALUES ('admin', '8d969eef6ecad3c29a3a629280e686cf0c3f5d5a86aff3ca12020c923adc6c92', 'administrador');

5. Verificar que las tablas `Usuarios`, `Productos`, `Ventas`, etc., estén creadas correctamente.

---

## ▶️ Ejecutar el proyecto

1. Clonar el repositorio:
   ```bash
   git clone https://github.com/cristiancb10/simcov.git
2. Abrir el proyecto con Visual Studio.

3. Configurar la cadena de conexión (clases/conexion_simcov.cs).

4. Compilar y ejecutar (Ctrl + F5).

Cristian Coca
Proyecto desarrollado como parte del curso de Programacion de aplicacion de escritorio en la Universidad Autónoma Tomas Frías.
