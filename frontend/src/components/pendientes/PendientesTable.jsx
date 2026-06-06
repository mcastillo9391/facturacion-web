export default function PendientesTable({
  pendientes,
  onEditar,
  onFacturar,
  onCancelar,
}) {
  return (
    <table>
      <thead>
        <tr>
          <th>ID</th>
          <th>Cliente</th>
          <th>Estado</th>
          <th>Total</th>
          <th>Acciones</th>
        </tr>
      </thead>

      <tbody>
        {pendientes.map((p) => (
          <tr key={p.pendienteVentaId}>
            <td>{p.pendienteVentaId}</td>

            <td>{p.cliente}</td>

            <td>{p.estado}</td>

            <td>
              $
              {Number(
                p.total
              ).toLocaleString()}
            </td>

            <td>
              {p.estado ===
                "ABIERTO" && (
                <>
                  <button
                    onClick={() =>
                      onEditar(
                        p.pendienteVentaId
                      )
                    }
                  >
                    Editar
                  </button>

                  <button
                    onClick={() =>
                      onFacturar(
                        p.pendienteVentaId
                      )
                    }
                  >
                    Facturar
                  </button>

                  <button
                    onClick={() =>
                      onCancelar(
                        p.pendienteVentaId
                      )
                    }
                  >
                    Cancelar
                  </button>
                </>
              )}
            </td>
          </tr>
        ))}
      </tbody>
    </table>
  );
}