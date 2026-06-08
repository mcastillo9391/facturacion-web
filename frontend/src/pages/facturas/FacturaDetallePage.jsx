    import {
useEffect,
useState
} from "react";

import {
useParams,
useNavigate
} from "react-router-dom";

import {
obtenerFactura
} from "../../services/facturaService";

import {
registrarPago
} from "../../services/pagoService";

export default function FacturaDetallePage() {

const { id } =
useParams();

const navigate =
useNavigate();

const [factura,
setFactura] =
useState(null);

const [valorPago,
setValorPago] =
useState("");

useEffect(() => {
cargarFactura();
}, []);

const cargarFactura =
async () => {

  const data =
    await obtenerFactura(id);

  setFactura(data);
};

const guardarPago =
async () => {

  if (
    !valorPago ||
    Number(valorPago) <= 0
  ) {
    alert(
      "Ingrese un valor válido"
    );
    return;
  }

  try {

    await registrarPago({
      facturaId:
        factura.codigo,
      valorPago:
        parseFloat(valorPago)
    });

    alert(
      "Pago registrado correctamente"
    );

    setValorPago("");

    cargarFactura();

  } catch (error) {

    alert(
      error?.response?.data?.message ||
      "Error registrando pago"
    );
  }
};

if (!factura)
return <p>Cargando...</p>;

const saldo =
factura.valorFactura -
(factura.valorAbonado || 0);

return (
  <div className="page-container">

    <div className="page-hero">
      <div>
        <h1>Factura #{factura.codigo}</h1>
        <p>
          Gestión de pagos y detalle de factura
        </p>
      </div>

      <button
        className="btn-view"
        onClick={() => navigate("/facturas")}
      >
        ← Volver
      </button>
    </div>

    <div className="cards-grid">

      <div className="card">
        <h3>Cliente</h3>
        <p>{factura.cliente}</p>
      </div>

      <div className="card">
        <h3>Estado</h3>

        <span
          className={`badge ${
            factura.estado === "PAGADA"
              ? "success"
              : "warning"
          }`}
        >
          {factura.estado}
        </span>
      </div>

      <div className="card">
        <h3>Total Factura</h3>
        <p>
          $
          {Number(
            factura.valorFactura
          ).toLocaleString()}
        </p>
      </div>

      <div className="card">
        <h3>Valor Abonado</h3>
        <p>
          $
          {Number(
            factura.valorAbonado || 0
          ).toLocaleString()}
        </p>
      </div>

      <div className="card">
        <h3>Saldo Pendiente</h3>
        <p>
          $
          {Number(
            saldo
          ).toLocaleString()}
        </p>
      </div>

    </div>

    <div className="detail-card">

      <h3
        style={{
          marginBottom: "20px"
        }}
      >
        Productos Facturados
      </h3>

      <div className="table-container">
        <div className="table-scroll">

          <table>
            <thead>
              <tr>
                <th>Producto</th>
                <th>Cantidad</th>
                <th>Valor Unitario</th>
                <th>Total</th>
              </tr>
            </thead>

            <tbody>

              {factura.detalles?.map(
                (d) => (
                  <tr key={d.codigo}>
                    <td>{d.producto}</td>

                    <td>{d.cantidad}</td>

                    <td>
                      $
                      {Number(
                        d.valorUnitario
                      ).toLocaleString()}
                    </td>

                    <td>
                      $
                      {Number(
                        d.total
                      ).toLocaleString()}
                    </td>
                  </tr>
                )
              )}

            </tbody>
          </table>

        </div>
      </div>

    </div>

    {factura.estado !== "PAGADA" && (

      <div
        className="detail-card"
        style={{
          marginTop: "24px"
        }}
      >
        <h3
          style={{
            marginBottom: "20px"
          }}
        >
          Registrar Pago
        </h3>

        <div
          style={{
            display: "grid",
            gridTemplateColumns:
              "1fr auto",
            gap: "12px",
            alignItems: "center",
          }}
        >

          <input
            type="number"
            placeholder="Valor a pagar"
            value={valorPago}
            onChange={(e) =>
              setValorPago(
                e.target.value
              )
            }
          />

          <button
            className="btn-success"
            onClick={guardarPago}
          >
            Registrar Pago
          </button>

        </div>
      </div>

    )}

  </div>
);
}
