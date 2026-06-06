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

  const [envios, setEnvios] =
    useState([]);

  const [busqueda, setBusqueda] =
    useState("");

  const [modoEdicion, setModoEdicion] =
    useState(false);

  const [envioSeleccionado,
    setEnvioSeleccionado] =
    useState(null);

  const [formData, setFormData] =
    useState({
      ciudad: "",
      valorEnvio: ""
    });

  const cargarEnvios =
    async () => {

      const data =
        await obtenerEnvios();

      setEnvios(data);
    };

  useEffect(() => {
    cargarEnvios();
  }, []);

  const limpiarFormulario =
    () => {

      setModoEdicion(false);

      setEnvioSeleccionado(
        null
      );

      setFormData({
        ciudad: "",
        valorEnvio: ""
      });
    };

  const guardar =
    async (e) => {

      e.preventDefault();

      if (
        !formData.ciudad.trim()
      ) {
        alert(
          "Debe ingresar la ciudad."
        );
        return;
      }

      if (
        !formData.valorEnvio ||
        parseFloat(
          formData.valorEnvio
        ) <= 0
      ) {
        alert(
          "Debe ingresar un valor válido."
        );
        return;
      }

      const payload = {
        ciudad:
          formData.ciudad,
        valorEnvio:
          parseFloat(
            formData.valorEnvio
          )
      };

      if (
        modoEdicion
      ) {
        await actualizarEnvio(
          envioSeleccionado.codigo,
          payload
        );
      } else {
        await crearEnvio(
          payload
        );
      }

      limpiarFormulario();

      cargarEnvios();
    };

  const editar =
    (envio) => {

      setModoEdicion(true);

      setEnvioSeleccionado(
        envio
      );

      setFormData({
        ciudad:
          envio.ciudad,
        valorEnvio:
          envio.valorEnvio
      });
    };

  const enviosFiltrados =
    envios.filter(
      (e) =>
        e.ciudad
          ?.toLowerCase()
          .includes(
            busqueda.toLowerCase()
          )
    );

    const {
        page,
        totalPages,
        paginatedData,
        nextPage,
        prevPage,
        goToPage,
        setPage
    } = usePagination(enviosFiltrados, 10);
    
    useEffect(() => {
    setPage(1);
    }, [busqueda]);
    
  const totalEnvios =
    enviosFiltrados.reduce(
      (acc, item) =>
        acc +
        item.valorEnvio,
      0
    );

  return (
    <div className="page-container">

      <div className="page-header">

        <h2>
          Gastos de Envío
        </h2>

        <h3>
          Total:
          {" "}
          $
          {totalEnvios.toLocaleString()}
        </h3>

      </div>

      <form
        onSubmit={guardar}
      >

        <input
          type="text"
          placeholder="Ciudad"
          value={
            formData.ciudad
          }
          onChange={(e) =>
            setFormData({
              ...formData,
              ciudad:
                e.target.value
            })
          }
        />

        <input
          type="number"
          placeholder="Valor envío"
          value={
            formData.valorEnvio
          }
          onChange={(e) =>
            setFormData({
              ...formData,
              valorEnvio:
                e.target.value
            })
          }
        />

        <button
          type="submit"
        >
          {modoEdicion
            ? "Actualizar"
            : "Registrar"}
        </button>

        {modoEdicion && (
          <button
            type="button"
            onClick={
              limpiarFormulario
            }
          >
            Cancelar
          </button>
        )}

      </form>

      <br />

      <input
        type="text"
        placeholder="Buscar ciudad..."
        value={busqueda}
        onChange={(e) =>
          setBusqueda(
            e.target.value
          )
        }
      />

      <table>

        <thead>
          <tr>

            <th>
              Código
            </th>

            <th>
              Fecha
            </th>

            <th>
              Ciudad
            </th>

            <th>
              Valor
            </th>

            <th>
              Acción
            </th>

          </tr>
        </thead>

        <tbody>

          {paginatedData.map(
            (envio) => (

              <tr
                key={
                  envio.codigo
                }
              >

                <td>
                  {
                    envio.codigo
                  }
                </td>

                <td>
                  {
                    new Date(
                      envio.fechaEnvio
                    )
                    .toLocaleDateString()
                  }
                </td>

                <td>
                  {
                    envio.ciudad
                  }
                </td>

                <td>
                  $
                  {envio.valorEnvio
                    .toLocaleString()}
                </td>

                <td>

                  <button
                    onClick={() =>
                      editar(
                        envio
                      )
                    }
                  >
                    Editar
                  </button>

                </td>

              </tr>

            )
          )}

        </tbody>

      </table>
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