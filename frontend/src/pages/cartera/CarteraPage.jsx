import {
  useEffect,
  useState
} from "react";

import { usePagination } from "../../hooks/usePagination";
import Pagination from "../../components/Pagination";

import {
  obtenerCartera
} from "../../services/carteraService";

export default function CarteraPage() {

  const [facturas,
    setFacturas] =
    useState([]);

  const [busqueda,
    setBusqueda] =
    useState("");

  const cargarDatos =
    async () => {

      const data =
        await obtenerCartera();

      setFacturas(data);
    };

  useEffect(() => {
    cargarDatos();
  }, []);

  const facturasFiltradas =
    facturas.filter(
      (f) =>
        f.cliente
          .toLowerCase()
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
    } = usePagination(facturasFiltradas, 10);
        
    useEffect(() => {
    setPage(1);
    }, [busqueda]);

  const totalCartera =
    facturasFiltradas.reduce(
      (acc, item) =>
        acc + item.saldo,
      0
    );

  return (
    <div className="page-container">

      <div className="page-header">

        <h2>
          Cartera
        </h2>

        <h3>
          Total:
          {" "}
          $
          {totalCartera
            .toLocaleString()}
        </h3>

      </div>

      <input
        type="text"
        placeholder="Buscar cliente..."
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
              Factura
            </th>

            <th>
              Cliente
            </th>

            <th>
              Fecha
            </th>

            <th>
              Valor
            </th>

            <th>
              Abonado
            </th>

            <th>
              Saldo
            </th>

            <th>
              Estado
            </th>

          </tr>
        </thead>

        <tbody>

          {paginatedData.map(
            (f) => (

              <tr
                key={
                  f.facturaId
                }
              >

                <td>
                  {
                    f.facturaId
                  }
                </td>

                <td>
                  {
                    f.cliente
                  }
                </td>

                <td>
                  {
                    new Date(
                      f.fechaFactura
                    )
                    .toLocaleDateString()
                  }
                </td>

                <td>
                  $
                  {f.valorFactura
                    .toLocaleString()}
                </td>

                <td>
                  $
                  {f.valorAbonado
                    .toLocaleString()}
                </td>

                <td>
                  $
                  {f.saldo
                    .toLocaleString()}
                </td>

                <td>
                  {
                    f.estado
                  }
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