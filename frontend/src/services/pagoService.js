import api from "../api/axios";

export const obtenerPagos =
  async () => {
    const response =
      await api.get("/pagos");

    return response.data;
  };

export const obtenerPago =
  async (id) => {
    const response =
      await api.get(
        `/pagos/${id}`
      );

    return response.data;
  };

export const registrarPago =
  async (data) => {
    const response =
      await api.post(
        "/pagos",
        data
      );

    return response.data;
  };

export const anularPago =
  async (id) => {
    const response =
      await api.put(
        `/pagos/${id}/anular`
      );

    return response.data;
  };