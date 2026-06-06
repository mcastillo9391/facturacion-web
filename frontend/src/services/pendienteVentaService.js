import api from "../api/axios";

export const obtenerPendientes = async () => {
  const response =
    await api.get("/PendientesVenta");

  return response.data;
};

export const obtenerPendiente = async (id) => {
  const response =
    await api.get(`/PendientesVenta/${id}`);

  return response.data;
};

export const crearPendiente = async (data) => {
  const response =
    await api.post(
      "/PendientesVenta",
      data
    );

  return response.data;
};

export const agregarProductoPendiente =
  async (id, data) => {
    await api.post(
      `/PendientesVenta/${id}/productos`,
      data
    );
  };

export const eliminarProductoPendiente =
  async (detalleId) => {
    await api.delete(
      `/PendientesVenta/detalles/${detalleId}`
    );
  };

export const cancelarPendiente =
  async (id) => {
    await api.put(
      `/PendientesVenta/${id}/cancelar`
    );
  };