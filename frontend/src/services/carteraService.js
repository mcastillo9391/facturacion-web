import api from "../api/axios";

export const obtenerCartera =
  async () => {
    const response =
      await api.get("/cartera");

    return response.data;
  };