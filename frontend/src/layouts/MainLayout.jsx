import { useState, useEffect } from "react";
import {
  Outlet,
  Link,
  useNavigate
} from "react-router-dom";

import { useAuth }
  from "../contexts/AuthContext";

import {
  obtenerNotificaciones,
  marcarLeida
} from "../services/notificacionService";

import "./MainLayout.css";

import FloatingChat
  from "../components/ai/FloatingChat";

export default function MainLayout() {
  const { user, logout } = useAuth();

  const navigate = useNavigate();

  const [sidebarOpen,
    setSidebarOpen] =
    useState(false);

  const [notificaciones,
    setNotificaciones] =
    useState([]);

  const [showNotifications,
    setShowNotifications] =
    useState(false);

  useEffect(() => {
    const interval = setInterval(() => {
      cargarNotificaciones();
    }, 30000);

    return () =>
      clearInterval(interval);
  }, []);

  const cargarNotificaciones =
    async () => {
      try {
        const data =
          await obtenerNotificaciones();

        setNotificaciones(
          data ?? []
        );
      } catch (error) {
        console.error(
          "Error cargando notificaciones",
          error
        );
      }
    };

  const abrirNotificacion =
    async (notificacion) => {

      await marcarLeida(
        notificacion.id
      );

      setNotificaciones(prev =>
        prev.map(n =>
          n.id === notificacion.id
            ? {
                ...n,
                leida: true
              }
            : n
        )
      );

      setShowNotifications(false);

      navigate("/pendientes");
    };


  const cantidadSinLeer =
    notificaciones.filter(
      n => !n.leida
    ).length;

  const cerrarSesion = () => {
    logout();
    navigate("/login");
  };

  return (
    <div className="layout">

      <aside
        className={`sidebar ${
          sidebarOpen ? "open" : ""
        }`}
      >
        <div className="sidebar-header">
          <h2>SUMPTUOUS</h2>

          <button
            className="close-btn"
            onClick={() =>
              setSidebarOpen(false)
            }
          >
            ✕
          </button>
        </div>

        <p className="user-name">
          {user?.nombre}
        </p>

        <nav>
          <Link to="/dashboard">
            Dashboard
          </Link>

          <Link to="/cartera">
            Cartera
          </Link>

          <Link to="/facturas">
            Facturas
          </Link>

          {(user?.rol ===
            "Administrador" ||
            user?.rol ===
            "Vendedor") && (
            <>
              <Link to="/clientes">
                Clientes
              </Link>

              <Link to="/productos">
                Productos
              </Link>

              <Link to="/envios">
                Envíos
              </Link>

              <Link to="/pendientes">
                Pendientes
              </Link>

              <Link to="/pagos">
                Pagos
              </Link>
            </>
          )}

          {user?.rol ===
            "Administrador" && (
            <Link to="/usuarios">
              Usuarios
            </Link>
          )}
        </nav>

        <button
          className="logout-btn"
          onClick={cerrarSesion}
        >
          Cerrar sesión
        </button>
      </aside>

      <div className="content-wrapper">

        <header className="topbar">

          <button
            className="menu-btn"
            onClick={() =>
              setSidebarOpen(true)
            }
          >
            ☰
          </button>

          <div>
            <h1>SUMPTUOUS</h1>

            <span
              style={{
                color: "#94a3b8",
                fontSize: "12px"
              }}
            >
              Sistema de Facturación
            </span>
          </div>

          <div
            style={{
              marginLeft: "auto",
              position: "relative"
            }}
          >
            <button
              onClick={() =>
                setShowNotifications(
                  !showNotifications
                )
              }
              style={{
                background: "none",
                border: "none",
                cursor: "pointer",
                fontSize: "22px",
                position: "relative"
              }}
            >
              🔔

              {cantidadSinLeer > 0 && (
                <span
                  style={{
                    position: "absolute",
                    top: "-6px",
                    right: "-8px",
                    background: "#ef4444",
                    color: "#fff",
                    borderRadius: "50%",
                    minWidth: "18px",
                    height: "18px",
                    fontSize: "11px",
                    display: "flex",
                    alignItems:
                      "center",
                    justifyContent:
                      "center"
                  }}
                >
                  {cantidadSinLeer}
                </span>
              )}
            </button>

            {showNotifications && (
              <div
                style={{
                  position: "absolute",
                  right: 0,
                  top: "40px",
                  width: "320px",
                  maxHeight: "400px",
                  overflowY: "auto",
                  background: "#0f172a",
                  border: "1px solid #334155",
                  borderRadius: "10px",
                  boxShadow:
                    "0 10px 25px rgba(0,0,0,.4)",
                  zIndex: 9999,
                  color: "#e2e8f0"
                }}
              >
                <div
                  style={{
                    padding: "12px",
                    borderBottom:
                      "1px solid #334155",
                    fontWeight: 600,
                    color: "#f8fafc"
                  }}
                >
                  Notificaciones
                </div>

                {notificaciones.length === 0 ? (
                  <div
                    style={{
                      padding: "16px",
                      color: "#94a3b8"
                    }}
                  >
                    No tienes notificaciones.
                  </div>
                ) : (
                  notificaciones.map(n => (
                    <div
                      key={n.id}
                      onClick={() =>
                        abrirNotificacion(n)
                      }
                      style={{
                        cursor: "pointer",
                        padding: "12px",
                        borderBottom:
                          "1px solid #334155",
                        background:
                          n.leida
                            ? "#0f172a"
                            : "#1e293b"
                      }}
                    >
                      <div
                        style={{
                          fontWeight: 600,
                          color: "#f8fafc"
                        }}
                      >
                        {n.titulo}
                      </div>

                      <div
                        style={{
                          fontSize: "13px",
                          marginTop: "4px",
                          color: "#cbd5e1"
                        }}
                      >
                        {n.mensaje}
                      </div>

                      <div
                        style={{
                          marginTop: "6px",
                          fontSize: "11px",
                          color: "#94a3b8"
                        }}
                      >
                        {new Date(
                          n.fecha
                        ).toLocaleString("es-CO")}
                      </div>
                    </div>
                  ))
                )}
              </div>
            )}
          </div>

        </header>

        <main className="content">
          <Outlet />
        </main>

        <FloatingChat />

      </div>
    </div>
  );
}