import {
useEffect,
useState
} from "react";

import {
obtenerDashboard
} from "../../services/dashboardService";

export default function DashboardPage() {

const [datos,
setDatos] =
useState(null);

useEffect(() => {

const cargar =
  async () => {

    try {

      const response =
        await obtenerDashboard();

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

  <div className="cards-grid">

    <div className="card">
      <h3>Ventas Hoy</h3>
      <p>
        $
        {Number(
          datos.ventasHoy
        ).toLocaleString()}
      </p>
    </div>

    <div className="card">
      <h3>Ventas Mes</h3>
      <p>
        $
        {Number(
          datos.ventasMes
        ).toLocaleString()}
      </p>
    </div>

    <div className="card">
      <h3>Recaudos Hoy</h3>
      <p>
        $
        {Number(
          datos.recaudosHoy
        ).toLocaleString()}
      </p>
    </div>

    <div className="card">
      <h3>Recaudos Mes</h3>
      <p>
        $
        {Number(
          datos.recaudosMes
        ).toLocaleString()}
      </p>
    </div>

    <div className="card">
      <h3>Gastos Envío</h3>
      <p>
        $
        {Number(
          datos.gastosEnvioMes
        ).toLocaleString()}
      </p>
    </div>

    <div className="card">
      <h3>Cartera</h3>
      <p>
        $
        {Number(
          datos.carteraPendiente
        ).toLocaleString()}
      </p>
    </div>

    <div className="card">
      <h3>Facturas Pendientes</h3>
      <p>
        {datos.facturasPendientes}
      </p>
    </div>

    <div className="card">
      <h3>Clientes Activos</h3>
      <p>
        {datos.clientesActivos}
      </p>
    </div>

    <div className="card">
      <h3>Productos Activos</h3>
      <p>
        {datos.productosActivos}
      </p>
    </div>

  </div>

</div>

);
}
