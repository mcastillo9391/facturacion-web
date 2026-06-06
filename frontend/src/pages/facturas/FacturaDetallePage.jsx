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

return ( <div className="page-container">

  <button
    onClick={() =>
      navigate("/facturas")
    }
  >
    ← Volver
  </button>

  <h2>
    Factura #
    {factura.codigo}
  </h2>

  <hr />

  <p>
    <strong>
      Cliente:
    </strong>
    {" "}
    {factura.cliente}
  </p>

  <p>
    <strong>
      Estado:
    </strong>
    {" "}
    {factura.estado}
  </p>

  <p>
    <strong>
      Total:
    </strong>
    {" "}
    $
    {Number(
      factura.valorFactura
    ).toLocaleString()}
  </p>

  <p>
    <strong>
      Abonado:
    </strong>
    {" "}
    $
    {Number(
      factura.valorAbonado || 0
    ).toLocaleString()}
  </p>

  <p>
    <strong>
      Saldo:
    </strong>
    {" "}
    $
    {Number(
      saldo
    ).toLocaleString()}
  </p>

  <hr />

  <h3>
    Productos
  </h3>

  <table>

    <thead>
      <tr>
        <th>
          Producto
        </th>

        <th>
          Cantidad
        </th>

        <th>
          Valor
        </th>

        <th>
          Total
        </th>
      </tr>
    </thead>

    <tbody>

      {
        factura.detalles?.map(
          d => (
            <tr
              key={d.codigo}
            >

              <td>
                {
                  d.producto
                }
              </td>

              <td>
                {
                  d.cantidad
                }
              </td>

              <td>
                $
                {
                  Number(
                    d.valorUnitario
                  )
                  .toLocaleString()
                }
              </td>

              <td>
                $
                {
                  Number(
                    d.total
                  )
                  .toLocaleString()
                }
              </td>

            </tr>
          )
        )
      }

    </tbody>

  </table>

  {
    factura.estado !==
    "PAGADA" && (

      <>
        <hr />

        <h3>
          Registrar Pago
        </h3>

        <input
          type="number"
          value={valorPago}
          onChange={(e) =>
            setValorPago(
              e.target.value
            )
          }
          placeholder="Valor a pagar"
        />

        <button
          onClick={
            guardarPago
          }
        >
          Registrar Pago
        </button>

      </>
    )
  }

</div>

);
}
