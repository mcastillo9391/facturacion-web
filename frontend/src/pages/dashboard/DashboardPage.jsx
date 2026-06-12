import {
useEffect,
useState
} from "react";

import {
obtenerDashboard
} from "../../services/dashboardService";
import "./DashboardPage.css";
import { useAuth } from "../../contexts/AuthContext";

export default function DashboardPage() {
const { user } = useAuth();

const esConsulta =
  user?.rol === "Consulta";
const [datos,
setDatos] =
useState(null);

useEffect(() => {

const cargar =
  async () => {

    try {

      const response =
        await obtenerDashboard();
        console.log("DASHBOARD RESPONSE:", response);
      setDatos(response);

    } catch (error) {

      console.error(error);

    }

  };

cargar();

}, []);

if (!datos) {

return (
  <div>
    Cargando dashboard...
  </div>
);

}

return (

<div className="dashboard-container">

  <h2>
    Dashboard
  </h2>

  <div className="dashboard-grid">

    <div className="kpi-card">
      <h3>📄 Facturas Pendientes</h3>
      <span>{datos.facturasPendientes}</span>
    </div>

    <div className="kpi-card">
      <h3>💰 Ventas Mes</h3>
      <span>$
        {Number(
          datos.ventasMes
        ).toLocaleString()}</span>
    </div>
    {!esConsulta && (
      <div className="kpi-card">
        <h3>🏦 Recaudos Hoy</h3>
        <span>${Number(datos.recaudosHoy).toLocaleString()}</span>
      </div>
    )}
    <div className="kpi-card">
      <h3>📊 Cartera</h3>
      <span>${Number(datos.carteraPendiente).toLocaleString()}</span>
    </div>
    

  </div>
  
</div>

);
}
