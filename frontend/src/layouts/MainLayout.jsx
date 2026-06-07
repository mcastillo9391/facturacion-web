import { useAuth }
from "../contexts/AuthContext";
import { Link, Outlet, useNavigate }
from "react-router-dom";

export default function MainLayout() {

const navigate = useNavigate();

const cerrarSesion = () => {
  logout();
  navigate("/login");
};

const { user, logout } =
useAuth();

return (
<div
style={{
display: "flex",
minHeight: "100vh",
}}
>
<aside
style={{
width: "250px",
background: "#1f2937",
color: "white",
padding: "20px",
}}
> <h2>
SUMPTUOUS </h2>

    <p>
      {user?.nombre}
    </p>

    <hr />

    <nav
        style={{
            display: "flex",
            flexDirection: "column",
            gap: "10px",
        }}
        >

        <Link to="/dashboard">
            Dashboard
        </Link>

        {user?.rol === "Administrador" && (
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

            <Link to="/facturas">
                Facturas
            </Link>

            <Link to="/pagos">
                Pagos
            </Link>

            <Link to="/cartera">
                Cartera
            </Link>

            <Link to="/usuarios">
                Usuarios
            </Link>
            
            </>
        )}

        {user?.rol === "Vendedor" && (
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

            <Link to="/facturas">
                Facturas
            </Link>

            <Link to="/pagos">
                Pagos
            </Link>

            <Link to="/cartera">
                Cartera
            </Link>
            </>
            
        )}

        {user?.rol === "Consulta" && (
            <>
            <Link to="/cartera">
                Cartera
            </Link>
            </>
        )}

    </nav>

    <br />

    <button
        onClick={cerrarSesion}
        >
        Salir
    </button>
  </aside>

  <main
    style={{
      flex: 1,
      padding: "20px",
    }}
  >
    <Outlet />
  </main>
</div>

);
}
