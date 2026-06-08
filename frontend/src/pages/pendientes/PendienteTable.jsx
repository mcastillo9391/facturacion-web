export default function PendienteTable({
  pendientes,
  onEditar,
  onFacturar,
  onCancelar,
}) {
  const obtenerClaseEstado = (estado) => {
    const valor = estado?.toUpperCase();

    if (valor === "FACTURADA") return "success";
    if (valor === "ABIERTO") return "warning";

    return "danger";
  };

  return (
    <div className="table-container">
      <div className="table-scroll">
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
            {pendientes.length === 0 ? (
              <tr>
                <td
                  colSpan="5"
                  style={{
                    textAlign: "center",
                    padding: "30px",
                  }}
                >
                  No hay pendientes registrados
                </td>
              </tr>
            ) : (
              pendientes.map((p) => (
                <tr key={p.pendienteVentaId}>
                  <td>{p.pendienteVentaId}</td>

                  <td>{p.cliente}</td>

                  <td>
                    <span
                      className={`badge ${obtenerClaseEstado(
                        p.estado
                      )}`}
                    >
                      {p.estado}
                    </span>
                  </td>

                  <td>
                    $
                    {Number(
                      p.total || 0
                    ).toLocaleString()}
                  </td>

                  <td>
                    <div className="actions">
                      {p.estado
                        ?.trim()
                        ?.toUpperCase() ===
                      "ABIERTO" ? (
                        <>
                          <button
                            className="btn-view"
                            onClick={() =>
                              onEditar(
                                p.pendienteVentaId
                              )
                            }
                          >
                            Editar
                          </button>

                          <button
                            className="btn-success"
                            onClick={() =>
                              onFacturar(
                                p.pendienteVentaId
                              )
                            }
                          >
                            Facturar
                          </button>

                          <button
                            className="btn-danger"
                            onClick={() =>
                              onCancelar(
                                p.pendienteVentaId
                              )
                            }
                          >
                            Cancelar
                          </button>
                        </>
                      ) : (
                        <button
                          className="btn-view"
                          onClick={() =>
                            onEditar(
                              p.pendienteVentaId
                            )
                          }
                        >
                          Ver
                        </button>
                      )}
                    </div>
                  </td>
                </tr>
              ))
            )}
          </tbody>
        </table>
      </div>
    </div>
  );
}