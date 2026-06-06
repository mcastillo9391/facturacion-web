import { useState, useMemo, useEffect } from "react";

export function usePagination(data = [], itemsPerPage = 10) {
  const [page, setPage] = useState(1);
  

  const totalPages = Math.ceil(data.length / itemsPerPage);

  const paginatedData = useMemo(() => {
    const start = (page - 1) * itemsPerPage;
    return data.slice(start, start + itemsPerPage);
  }, [data, page, itemsPerPage]);

  const nextPage = () => {
    setPage((p) => Math.min(p + 1, totalPages));
  };

  const prevPage = () => {
    setPage((p) => Math.max(p - 1, 1));
  };

  const goToPage = (n) => {
    setPage(Math.max(1, Math.min(n, totalPages)));
  };

  return {
    page,
    totalPages,
    paginatedData,
    nextPage,
    prevPage,
    goToPage,
    setPage
  };
}