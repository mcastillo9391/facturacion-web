import { useState } from "react";
import { Outlet, Link, useNavigate } from "react-router-dom";
import { useAuth } from "../contexts/AuthContext";
import "./MainLayout.css";

export default function MainLayout() {
  const { user, logout } = useAuth();
  const navigate = useNavigate();

  const [sidebarOpen, setSidebarOpen] = useState(false);

  const cerrarSesion = () => {
    logout();
    navigate("/login");
  };

  return (
    <div className="layout">
      <aside className={`sidebar ${sidebarOpen ? "open" : ""}`}>
        <div className="sidebar-header">
          <h2>SUMPTUOUS</h2>
          <button
            className="close-btn"
            onClick={() => setSidebarOpen(false)}
          >
            ✕
          </button>
        </div>

        <p className="user-name">{user?.nombre}</p>

        <nav>
          <Link to="/dashboard">Dashboard</Link>

          {(user?.rol === "Administrador" ||
            user?.rol === "Vendedor") && (
            <>
              <Link to="/clientes">Clientes</Link>
              <Link to="/productos">Productos</Link>
              <Link to="/envios">Envíos</Link>
              <Link to="/pendientes">Pendientes</Link>
              <Link to="/facturas">Facturas</Link>
              <Link to="/pagos">Pagos</Link>
              <Link to="/cartera">Cartera</Link>
            </>
          )}

          {user?.rol === "Administrador" && (
            <Link to="/usuarios">Usuarios</Link>
          )}
        </nav>

        <button className="logout-btn" onClick={cerrarSesion}>
          Cerrar sesión
        </button>
      </aside>

      <div className="content-wrapper">
        <header className="topbar">
          <button
            className="menu-btn"
            onClick={() => setSidebarOpen(true)}
          >
            ☰
          </button>

          <h1>Facturación Web</h1>
        </header>

        <main className="content">
          <Outlet />
        </main>
      </div>
    </div>
  );
}