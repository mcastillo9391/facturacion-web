import {
  BrowserRouter,
  Routes,
  Route,
  Navigate,
} from "react-router-dom";

import LoginPage from "../pages/auth/LoginPage";

import DashboardPage from "../pages/dashboard/DashboardPage";
import UsuariosPage from "../pages/usuarios/UsuariosPage";
import ClientesPage from "../pages/clientes/ClientesPage";
import ProductosPage from "../pages/productos/ProductosPage";
import FacturasPage from "../pages/facturas/FacturasPage";
import FacturaDetallePage from "../pages/facturas/FacturaDetallePage";
import PagosPage from "../pages/pagos/PagosPage";
import EnviosPage from "../pages/envios/EnviosPage";
import PendientesPage from "../pages/pendientes/PendientesPage";
import CarteraPage from "../pages/cartera/CarteraPage";

import PrivateRoute from "./PrivateRoute";
import RoleRoute from "./RoleRoute";

import MainLayout from "../layouts/MainLayout";

export default function AppRouter() {
  return (
    <BrowserRouter>
      <Routes>

        {/* Login */}
        <Route
          path="/login"
          element={<LoginPage />}
        />

        {/* Rutas protegidas */}
        <Route
          element={
            <PrivateRoute>
              <MainLayout />
            </PrivateRoute>
          }
        >

          <Route
            path="/dashboard"
            element={<DashboardPage />}
          />

          <Route
            path="/clientes"
            element={
              <RoleRoute roles={["Administrador",
                  "Vendedor",]}>
                <ClientesPage />
              </RoleRoute>
            }
          />

          <Route
            path="/usuarios"
            element={
              <RoleRoute roles={["Administrador"]}>
                <UsuariosPage />
              </RoleRoute>
            }
          />

          <Route
            path="/productos"
            element={
              <RoleRoute roles={["Administrador",
                  "Vendedor",]}>
                <ProductosPage />
              </RoleRoute>
            }
          />

          <Route
            path="/facturas"
            element={
              <RoleRoute
                roles={[
                  "Administrador",
                  "Vendedor",
                  "Consulta",
                ]}
              >
                <FacturasPage />
              </RoleRoute>
            }
          />

          <Route
            path="/facturas/:id"
            element={
              <RoleRoute
                roles={[
                  "Administrador",
                  "Vendedor",
                  "Consulta",
                ]}
              >
                <FacturaDetallePage />
              </RoleRoute>
            }
          />

          <Route
            path="/pagos"
            element={
              <RoleRoute
                roles={[
                  "Administrador",
                  "Vendedor",
                ]}
              >
                <PagosPage />
              </RoleRoute>
            }
          />

          <Route
            path="/pendientes"
            element={
              <RoleRoute
                roles={[
                  "Administrador",
                  "Vendedor",
                ]}
              >
                <PendientesPage />
              </RoleRoute>
            }
          />

          <Route
            path="/envios"
            element={
              <RoleRoute
                roles={[
                  "Administrador",
                  "Vendedor",
                ]}
              >
                <EnviosPage />
              </RoleRoute>
            }
          />

          <Route
            path="/cartera"
            element={
              <RoleRoute
                roles={[
                  "Administrador",
                  "Vendedor",
                  "Consulta",
                ]}
              >
                <CarteraPage />
              </RoleRoute>
            }
          />

        </Route>

        <Route
          path="*"
          element={
            <Navigate
              to="/dashboard"
              replace
            />
          }
        />

      </Routes>
    </BrowserRouter>
  );
}