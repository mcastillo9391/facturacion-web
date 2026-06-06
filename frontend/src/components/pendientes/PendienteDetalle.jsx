export default function PendienteDetalle({
  pendiente,
  productos,
  productoId,
  setProductoId,
  cantidad,
  setCantidad,
  descuento,
  setDescuento,
  agregarProducto,
  eliminarProducto,
}) {
  if (!pendiente)
    return null;

  return (
    <>
      <h3>
        Cliente:
        {" "}
        {pendiente.cliente}
      </h3>

      <div>
        <select
          value={productoId}
          onChange={(e) =>
            setProductoId(
              e.target.value
            )
          }
        >
          <option value="">
            Producto
          </option>

          {productos.map(
            (p) => (
              <option
                key={
                  p.idProducto
                }
                value={
                  p.idProducto
                }
              >
                {p.nombre}
              </option>
            )
          )}
        </select>

        <input
          type="number"
          value={cantidad}
          onChange={(e) =>
            setCantidad(
              e.target.value
            )
          }
        />

        <input
          type="number"
          value={descuento}
          onChange={(e) =>
            setDescuento(
              e.target.value
            )
          }
        />

        <button
          onClick={
            agregarProducto
          }
        >
          Agregar
        </button>
      </div>

      <table>
        <thead>
          <tr>
            <th>Producto</th>
            <th>Cantidad</th>
            <th>Valor</th>
            <th>Descuento</th>
            <th>Total</th>
            <th></th>
          </tr>
        </thead>

        <tbody>
          {pendiente.detalles.map(
            (d) => (
              <tr
                key={
                  d.detallePendienteVentaId
                }
              >
                <td>
                  {d.producto}
                </td>

                <td>
                  {d.cantidad}
                </td>

                <td>
                  {
                    d.valorUnitario
                  }
                </td>

                <td>
                  {d.descuento}
                </td>

                <td>
                  {d.total}
                </td>

                <td>
                  <button
                    onClick={() =>
                      eliminarProducto(
                        d.detallePendienteVentaId
                      )
                    }
                  >
                    Eliminar
                  </button>
                </td>
              </tr>
            )
          )}
        </tbody>
      </table>

      <h3>
        Total:
        {" "}
        {pendiente.total}
      </h3>
    </>
  );
}