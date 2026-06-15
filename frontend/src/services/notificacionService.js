import api from "../api/axios";

export const obtenerNotificaciones =
  async () => {
    const response =
      await api.get(
        "/notificaciones"
      );

    return response.data;
  };

export const marcarLeida =
  async (id) => {
    await api.put(
      `/notificaciones/${id}/leer`
    );
  };