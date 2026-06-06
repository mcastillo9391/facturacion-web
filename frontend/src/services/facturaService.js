import api from "../api/axios";

export const obtenerFacturas =
  async () => {
    const response =
      await api.get("/facturas");

    return response.data;
  };

export const obtenerFactura =
  async (id) => {
    const response =
      await api.get(
        `/facturas/${id}`
      );

    return response.data;
  };

export const generarFactura =
  async (pendienteId) => {
    const response =
      await api.post(
        `/facturas/generar/${pendienteId}`
      );

    return response.data;
  };