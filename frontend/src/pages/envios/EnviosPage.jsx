import {
  useEffect,
  useState
} from "react";

import { usePagination } from "../../hooks/usePagination";
import Pagination from "../../components/Pagination";

import {
  obtenerEnvios,
  crearEnvio,
  actualizarEnvio
} from "../../services/envioService";

export default function EnviosPage() {
  const [envios, setEnvios] = useState([]);
  const [busqueda, setBusqueda] = useState("");

  const [modoEdicion, setModoEdicion] =
    useState(false);

  const [envioSeleccionado, setEnvioSeleccionado] =
    useState(null);

  const [formData, setFormData] =
    useState({
      ciudad: "",
      valorEnvio: ""
    });

  const cargarEnvios = async () => {
    const data = await obtenerEnvios();
    setEnvios(data);
  };

  useEffect(() => {
    cargarEnvios();
  }, []);

  const limpiarFormulario = () => {
    setModoEdicion(false);

    setEnvioSeleccionado(null);

    setFormData({
      ciudad: "",
      valorEnvio: ""
    });
  };

  const guardar = async (e) => {
    e.preventDefault();

    if (!formData.ciudad.trim()) {
      alert("Debe ingresar la ciudad.");
      return;
    }

    if (
      !formData.valorEnvio ||
      parseFloat(formData.valorEnvio) <= 0
    ) {
      alert(
        "Debe ingresar un valor válido."
      );
      return;
    }

    const payload = {
      ciudad: formData.ciudad,
      valorEnvio: parseFloat(
        formData.valorEnvio
      )
    };

    try {
      if (modoEdicion) {
        await actualizarEnvio(
          envioSeleccionado.codigo,
          payload
        );
      } else {
        await crearEnvio(payload);
      }

      limpiarFormulario();
      cargarEnvios();

    } catch {
      alert(
        "Ocurrió un error guardando el envío."
      );
    }
  };

  const editar = (envio) => {
    setModoEdicion(true);

    setEnvioSeleccionado(envio);

    setFormData({
      ciudad: envio.ciudad,
      valorEnvio: envio.valorEnvio
    });
  };

  const enviosFiltrados = envios.filter(
    (e) =>
      e.ciudad
        ?.toLowerCase()
        .includes(busqueda.toLowerCase())
  );

  const {
    page,
    totalPages,
    paginatedData,
    nextPage,
    prevPage,
    goToPage,
    setPage
  } = usePagination(
    enviosFiltrados,
    10
  );

  useEffect(() => {
    setPage(1);
  }, [busqueda, setPage]);

  const totalEnvios =
    enviosFiltrados.reduce(
      (acc, item) =>
        acc + Number(item.valorEnvio || 0),
      0
    );

  return (
    <div className="page-container">

      {/* HERO */}
      <div className="page-hero">

        <div>
          <h1>🚚 Gastos de Envío</h1>

          <p>
            Administración de costos de transporte
          </p>
        </div>

        <div>
          <h2>
            $
            {totalEnvios.toLocaleString()}
          </h2>
        </div>

      </div>

      {/* FORMULARIO */}
      <form
        onSubmit={guardar}
        className="cliente-form"
      >

        <input
          type="text"
          placeholder="Ciudad"
          value={formData.ciudad}
          onChange={(e) =>
            setFormData({
              ...formData,
              ciudad: e.target.value
            })
          }
          required
        />

        <input
          type="number"
          placeholder="Valor envío"
          value={formData.valorEnvio}
          onChange={(e) =>
            setFormData({
              ...formData,
              valorEnvio: e.target.value
            })
          }
          required
        />

        <div>

          <button
            type="submit"
            className="btn-success"
          >
            {modoEdicion
              ? "Actualizar"
              : "Registrar"}
          </button>

          {modoEdicion && (
            <button
              type="button"
              className="btn-danger"
              onClick={
                limpiarFormulario
              }
            >
              Cancelar
            </button>
          )}

        </div>

      </form>

      {/* BUSCADOR */}
      <div className="search-container">

        <input
          type="text"
          placeholder="Buscar ciudad..."
          value={busqueda}
          onChange={(e) =>
            setBusqueda(e.target.value)
          }
        />

      </div>

      {/* TABLA */}
      <div className="table-container">

        <div className="table-scroll">

          <table>

            <thead>
              <tr>
                <th>Código</th>
                <th>Fecha</th>
                <th>Ciudad</th>
                <th>Valor</th>
                <th>Acción</th>
              </tr>
            </thead>

            <tbody>

              {paginatedData.length === 0 ? (

                <tr>
                  <td
                    colSpan="5"
                    style={{
                      textAlign: "center"
                    }}
                  >
                    No existen registros
                  </td>
                </tr>

              ) : (

                paginatedData.map(
                  (envio) => (

                    <tr
                      key={envio.codigo}
                    >

                      <td>
                        {envio.codigo}
                      </td>

                      <td>
                        {new Date(
                          envio.fechaEnvio
                        ).toLocaleDateString()}
                      </td>

                      <td>
                        {envio.ciudad}
                      </td>

                      <td>
                        $
                        {Number(
                          envio.valorEnvio
                        ).toLocaleString()}
                      </td>

                      <td>

                        <button
                          className="btn-view"
                          onClick={() =>
                            editar(envio)
                          }
                        >
                          Editar
                        </button>

                      </td>

                    </tr>

                  )
                )

              )}

            </tbody>

          </table>

        </div>

      </div>

      {/* PAGINACIÓN */}
      <Pagination
        page={page}
        totalPages={totalPages}
        nextPage={nextPage}
        prevPage={prevPage}
        goToPage={goToPage}
      />

    </div>
  );
}