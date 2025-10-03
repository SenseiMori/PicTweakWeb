import React, { useState, useEffect } from "react";
import axios from "axios";

interface TableRow {
    id: number;
    [key: string]: any;
}

interface Column<T> {
    header: string;
    accessor: (row: T) => React.ReactNode;
}

// Универсальная таблица
const AdminTable = <T extends TableRow>({ columns, data, title }: { columns: Column<T>[]; data: T[]; title?: string }) => (
    <div className="mb-5">
        {title && <h3 className="mb-3">{title}</h3>}
        <table className="table table-striped table-bordered">
            <thead className="table-light">
                <tr>{columns.map((col) => <th key={col.header}>{col.header}</th>)}</tr>
            </thead>
            <tbody>
                {data.map((row) => (
                    <tr key={row.id}>
                        {columns.map((col) => <td key={col.header}>{col.accessor(row)}</td>)}
                    </tr>
                ))}
            </tbody>
        </table>
    </div>
);

export const AdminPage: React.FC = () => {
    const [activeTab, setActiveTab] = useState<"logs" | "users" | "sessions">("logs");
    const [yesterdayData, setYesterdayData] = useState<TableRow[]>([]);
    const [todayData, setTodayData] = useState<TableRow[]>([]);
    const [data, setData] = useState<TableRow[]>([]);
    const [loading, setLoading] = useState(true);
    const [error, setError] = useState<string | null>(null);

    const columnsMap: Record<string, Column<TableRow>[]> = {
        logs: [
            { header: "ID", accessor: (row) => row.id },
            { header: "Пользователь", accessor: (row) => row.user },
            { header: "Действие", accessor: (row) => row.action },
            { header: "Дата", accessor: (row) => new Date(row.createdAt).toLocaleString() },
        ],
        users: [
            { header: "ID", accessor: (row) => row.id },
            { header: "Имя", accessor: (row) => row.name },
            { header: "Email", accessor: (row) => row.email },
            { header: "Роль", accessor: (row) => row.role },
        ],
        sessions: [
            { header: "ID", accessor: (row) => row.id },
            { header: "Пользователь", accessor: (row) => row.user },
            { header: "IP", accessor: (row) => row.ip },
            { header: "Начало сессии", accessor: (row) => new Date(row.startTime).toLocaleString() },
        ],
    };

    const fetchData = async (tab: string) => {
        setLoading(true);
        setError(null);
        try {
            if (tab === "logs") {
                const [yesterdayRes, todayRes] = await Promise.all([
                    axios.get("/api/admin/logs?date=yesterday"),
                    axios.get("/api/admin/logs?date=today")
                ]);

                setYesterdayData(yesterdayRes.data.map((log: any, index: number) => ({
                    id: log.Id ?? index,
                    user: log.Account?.NickName?.Value ?? "—",
                    action: log.Action ?? "—",
                    createdAt: log.SessionCreatedAt ?? log.CreatedAt ?? new Date()
                })));

                setTodayData(todayRes.data.map((log: any, index: number) => ({
                    id: log.Id ?? index,
                    user: log.Account?.NickName?.Value ?? "—",
                    action: log.Action ?? "—",
                    createdAt: log.SessionCreatedAt ?? log.CreatedAt ?? new Date()
                })));
            } else if (tab === "users") {
                const res = await axios.get("/api/admin/users");
                const usersData = res.data.map((u: any, index: number) => ({
                    id: u.Id ?? index,
                    name: u.NickName?.Value ?? u.Name ?? "—",
                    email: u.Email?.Value ?? u.Email ?? "—",
                    role: u.Role ?? "—"
                }));
                setData(usersData);
            } else if (tab === "sessions") {
                const res = await axios.get("/api/admin/sessions");
                const sessionsData = res.data.map((s: any, index: number) => ({
                    id: s.Id ?? index,
                    user: s.Account?.NickName?.Value ?? "—",
                    ip: s.IpAddress ?? s.IP ?? "—",
                    startTime: s.SessionCreatedAt ?? new Date()
                }));
                setData(sessionsData);
            }
        } catch (err: any) {
            setError(err.response?.data?.error || err.message);
        } finally {
            setLoading(false);
        }
    };

    useEffect(() => {
        fetchData(activeTab);
    }, [activeTab]);

    return (
        <div className="container mt-4">
            <h1 className="mb-4">Админ-панель</h1>

            <div className="mb-3">
                <button className="btn btn-primary me-2" onClick={() => setActiveTab("logs")}>Логи</button>
                <button className="btn btn-primary me-2" onClick={() => setActiveTab("users")}>Пользователи</button>
                <button className="btn btn-primary" onClick={() => setActiveTab("sessions")}>Сессии</button>
            </div>

            {loading && <div>Загрузка...</div>}
            {error && <div className="alert alert-danger">{error}</div>}

            {!loading && !error && activeTab === "logs" && (
                <>
                    <AdminTable columns={columnsMap.logs} data={yesterdayData} title="Логи за вчера" />
                    <AdminTable columns={columnsMap.logs} data={todayData} title="Логи за сегодня" />
                </>
            )}

            {!loading && !error && activeTab !== "logs" && (
                <AdminTable columns={columnsMap[activeTab]} data={data} />
            )}
        </div>
    );
};
