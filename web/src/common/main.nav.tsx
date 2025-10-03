import { Link } from "react-router-dom"; 
export const MainNav = () => {
    return (
        <nav className="navbar navbar-expand-lg navbar-light bg-light shadow-sm">
            <div className="container-fluid d-flex justify-content-between">
                <div className="d-flex align-items-center">
                    <Link
                        className="btn btn-outline-primary me-3"
                        to="/modifier"
                        style={{ fontSize: '1.1rem', fontWeight: '600' }}
                    >
                        WebPicTweak
                    </Link>
                    <Link className="navbar-brand" to="/auth">
                        Личный кабинет
                    </Link>
                    {/* <Link className="navbar-brand" to="/admin/logs">
                        Тестовая панель аналитики
                    </Link>*/}
                </div>

                <div className="d-flex align-items-center">
                </div>
            </div>
        </nav>
    );
};

