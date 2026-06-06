import api from "../api/axios";

export const obtenerDashboard =
async () => {

const response =
  await api.get("/dashboard");

return response.data;

};
