import React, { useState, useCallback, useEffect, useRef } from "react";
import { useDropzone } from "react-dropzone";
import { Button, ProgressBar, Modal } from "react-bootstrap";
import { toast, ToastContainer } from "react-toastify";
import { HubConnectionBuilder, HubConnection, HttpTransportType } from "@microsoft/signalr";
import { useNavigate } from "react-router-dom";
import "react-toastify/dist/ReactToastify.css";
import "./ModifierPage.css";
import { useAuth } from "../../common/services/api/auth/AuthContext";

type PendingUpload = {
    id: string; 
    file: File;
    preview: string | null;
    name: string;
    size: number;
    width?: number;
    height?: number;
    ext?: string | null;
    jobId?: string | null;
};


const resizeOptions = [0, 25, 50, 75] as const;
type ResizeOption = (typeof resizeOptions)[number];

const compressOptions = [0, 25, 50, 75] as const;
type CompressOption = (typeof compressOptions)[number];

const removeEXIFOptions = [true, false] as const;
type RemoveEXIFOption = (typeof removeEXIFOptions)[number];

const MAX_FILES = 10;

const ModifierPage: React.FC = () => {
    const [uploads, setUploads] = useState<PendingUpload[]>([]);
    const [resize, setResize] = useState<ResizeOption | null>(null);
    const [compress, setCompress] = useState<CompressOption | null>(null);
    const [removeEXIF, setRemoveEXIF] = useState<RemoveEXIFOption | null>(null);
    const [showModal, setShowModal] = useState(false);
    const [overallProgress, setOverallProgress] = useState(0);
    const [processedCount, setProcessedCount] = useState(0);
    const [totalCount, setTotalCount] = useState(0);
    const processedSetRef = useRef<Set<string>>(new Set());
    const [archiveReady, setArchiveReady] = useState(false);
    const [downloadUrl, setDownloadUrl] = useState<string | null>(null);
    {/*const [linkTTL, setLinkTTL] = useState<string>("15 минут");*/}
    const [downloadDisabled, setDownloadDisabled] = useState(false);
    const [isProcessing, setIsProcessing] = useState(false);
    const [removeHover, setRemoveHover] = useState(false);

    const navigate = useNavigate();
    const { isAuthorized, userId, guestId } = useAuth();

    const connectionRef = useRef<HubConnection | null>(null);
    const sessionIdRef = useRef<string | null>(null);

    const onDrop = useCallback(async (acceptedFiles: File[]) => {
        const newUploads = await Promise.all(
            acceptedFiles.map(async (file) => {
                const { width, height } = await getImageDimensions(file);
                return {
                    id: `${Date.now()}-${Math.random().toString(36).substr(2, 9)}`, 
                    file,
                    name: file.name,
                    size: file.size,
                    width,
                    height,
                    preview: URL.createObjectURL(file),
                };
            })
        );

        setUploads((prev) => {
            const combined = [...prev, ...newUploads];
            if (combined.length > MAX_FILES) {
                toast.warning(`Пока доступна обработка только ${MAX_FILES} картинок`);
                return combined.slice(0, MAX_FILES);
            }
            return combined;
        });
    }, []);


    const { getRootProps, getInputProps, open } = useDropzone({
        onDrop,
        multiple: true,
        accept: { "image/jpeg": [], "image/jpg": [] },
        noClick: true,
        noKeyboard: true,
    });

    useEffect(() => {
        let currentSessionId: string | null = null;

        if (isAuthorized && userId) {
            currentSessionId = userId;
        } else if (!isAuthorized && guestId) {
            currentSessionId = guestId;
            sessionIdRef.current = guestId;
        } else {
            console.log("Session ID is still loading or undetermined.");
            sessionIdRef.current = null;
            return;
        }
        sessionIdRef.current = currentSessionId;
        console.log("Session ID determined:", currentSessionId);
    }, [isAuthorized, guestId, userId]);

    useEffect(() => {
        const currentId = userId || guestId;
        if (!currentId) {
            console.log("⏳ Waiting for userId or guestId...");
            return;
        }

        let mounted = true;

        const initializeSignalR = async () => {
            const conn = new HubConnectionBuilder()
                .withUrl("http://localhost:5022/archiveHub", {
                    transport: HttpTransportType.WebSockets,
                })
                .withAutomaticReconnect()
                .build();

            connectionRef.current = conn;
            try {
                await conn.start();
                console.log("✅ SignalR connected");

                await conn.invoke("Register", currentId);
                console.log("✅ Registered in SignalR group:", currentId);
            } catch (err) {
                console.error("❌ SignalR error:", err);
                toast.error("Не удалось подключиться к серверу");
            }

            conn.on("ProgressUpdate", (payload) => {
                if (!mounted) return;
                if (!sessionIdRef.current || payload.userId !== sessionIdRef.current) return;

                if (typeof payload.percent === "number") {
                    setOverallProgress(Math.max(0, Math.min(100, Math.round(payload.percent))));
                }

                if (payload.fileName && !processedSetRef.current.has(payload.fileName)) {
                    if (payload.status?.toLowerCase() === "done" || payload.percent === 100) {
                        processedSetRef.current.add(payload.fileName);
                        setProcessedCount((c) => c + 1);
                    }
                }
            });

            conn.on("ArchiveReady", (payload) => {
                if (!mounted) return;
                if (!sessionIdRef.current || payload.userId.toString() !== sessionIdRef.current) return;

                setArchiveReady(true);
                setOverallProgress(100);
                setIsProcessing(false);
                setDownloadUrl(`/api/images/${payload.sessionId}`);
                {/*setLinkTTL(payload.expiresAt ?? "15 минут");*/ }
                toast.success("Архив готов к скачиванию!");
            });
        };

        initializeSignalR();

        return () => {
            mounted = false;
            if (connectionRef.current) {
                connectionRef.current.stop();
                connectionRef.current = null;
            }
        };
    }, [userId, guestId]);

    const getImageDimensions = (file: File): Promise<{ width: number; height: number }> => {
        return new Promise((resolve, reject) => {
            const img = new Image();
            img.onload = () => resolve({ width: img.width, height: img.height });
            img.onerror = reject;
            img.src = URL.createObjectURL(file);
        });
    };

    const handleProcess = async () => {
        if (!uploads.length) return;

        try {
            setIsProcessing(true);
            setShowModal(true);

            const formData = new FormData();

            uploads.forEach((u) => {
                formData.append("Files", u.file);
                formData.append("FileName", u.name);
                formData.append("Size", String(u.size));
                formData.append("Width", String(u.width));
                formData.append("Height", String(u.height));
            });

            if (sessionIdRef.current) {
                formData.append("user", sessionIdRef.current);
            }

            formData.append("Options.SizeScale", String(resize ?? 0));
            formData.append("Options.Compress", String(compress ?? 0));
            formData.append("Options.RemoveEXIF", String(removeEXIF ?? false));

            const resp = await fetch("/api/images", {
                method: "POST",
                body: formData,
            });

            if (!resp.ok) {
                const errorText = await resp.text();
                throw new Error(`Ошибка загрузки: ${resp.status} - ${errorText}`);
            }

            const result = await resp.json();
            console.log("Файлы успешно отправлены:", result);
            setTotalCount(uploads.length);
        } catch (err: any) {
            console.error("Process error:", err);
            toast.error(err.message || "Ошибка обработки");
            setIsProcessing(false);
        }
    };

    const clearUploads = () => {
        setUploads((prev) => {
            prev.forEach((upload) => {
                if (upload.preview) {
                    URL.revokeObjectURL(upload.preview);
                }
            });
            return [];
        });
    };


    const removeUpload = (id: string) => {
        setUploads((prev) => {
            const upload = prev.find((u) => u.id === id);
            if (upload?.preview) {
                URL.revokeObjectURL(upload.preview);
            }
            return prev.filter((u) => u.id !== id);
        });
    };



    const downloadArchive = (sessionIdParam: string) => {
        if (!sessionIdParam || downloadDisabled) {
            console.warn("Download blocked:", { sessionIdParam, downloadDisabled });
            return;
        }

        console.log("🔽 Starting download for sessionId:", sessionIdParam);
        setDownloadDisabled(true);

        try {
            const link = document.createElement("a");
            link.href = `/api/images/${sessionIdParam}`;
            link.download = "FromPicTweakWithLove.zip";
            document.body.appendChild(link);
            link.click();
            document.body.removeChild(link);

            toast.success("Скачивание началось");
        } catch (err) {
            console.error("❌ Download error:", err);
            toast.error("Ошибка при скачивании");
        } finally {
            setTimeout(() => setDownloadDisabled(false), 1000);
        }
    };

    const closeModal = () => {
        setShowModal(false);
        setIsProcessing(false);
        setOverallProgress(0);
        setProcessedCount(0);
        setArchiveReady(false);
        setDownloadUrl(null);
        {/*setLinkTTL(null);}*/}
        processedSetRef.current = new Set();
    };

    useEffect(() => {
        return () => {
            uploads.forEach((upload) => {
                if (upload.preview) {
                    URL.revokeObjectURL(upload.preview);
                }
            });
        };
    }, [uploads]);

    return (
        <div className="container-fluid">
            <ToastContainer position="top-right" autoClose={5000} />

            <div className="row">
                {/* LEFT COLUMN: Uploads */}
                <div className="col-md-8">
                    <div className="border rounded p-3 h-100 d-flex flex-column" {...getRootProps()}>
                        <input {...getInputProps()} />

                        
                        <div className="d-flex justify-content-between mb-3 sticky-top bg-white p-2 rounded border gap-2">
                            <div className="d-flex gap-2">
                                <Button variant="primary" size="sm" onClick={open}>
                                    Добавить файлы
                                </Button>
                                <Button variant="danger" size="sm" onClick={clearUploads} disabled={!uploads.length}>
                                    Удалить все
                                </Button>
                            </div>
                        </div>

                        {uploads.length ? (
                            <div className="row row-cols-2 row-cols-sm-3 row-cols-md-4 row-cols-lg-5 g-3 flex-grow-1 overflow-auto">
                                {uploads.map((u) => (
                                    <div key={u.id} className="col">
                                        <div className="card h-100 shadow-sm position-relative image-card">
                                            <div
                                                className="card-img-top bg-light d-flex align-items-center justify-content-center position-relative"
                                                style={{ height: 140, overflow: "hidden" }}
                                            >
                                                {u.preview ? (
                                                    <img
                                                        src={u.preview}
                                                        alt={u.name}
                                                        draggable={false}
                                                        onDragStart={(e) => e.preventDefault()}
                                                        style={{ width: "100%", height: "100%", objectFit: "cover" }}
                                                    />
                                                ) : (
                                                    <small className="text-muted">No preview</small>
                                                )}

                                                <button
                                                    onClick={() => removeUpload(u.id)}
                                                    className="remove-btn btn btn-sm btn-danger"
                                                    type="button"
                                                    aria-label={`Удалить ${u.name}`}
                                                >
                                                    ✖
                                                </button>
                                            </div>

                                            <div className="card-body py-2 px-2">
                                                <h6 className="card-title text-truncate mb-1" title={u.name}>
                                                    {u.name}
                                                </h6>
                                                <div className="small text-muted mb-2">
                                                    <div>
                                                        <strong>Размер:</strong> {u.width ?? "—"}×{u.height ?? "—"}
                                                    </div>
                                                    <div>
                                                        <strong>Вес:</strong> {(u.size / 1024).toFixed(1)} KB
                                                    </div>
                                                </div>
                                            </div>
                                        </div>
                                    </div>
                                ))}
                            </div>
                        ) : (
                            <div className="d-flex align-items-center justify-content-center h-100 text-muted">
                                <h5 className="mb-0">Перетащите сюда изображения</h5>
                            </div>
                        )}
                    </div>
                </div>

                    {/* RIGHT COLUMN: Options */}
                    <div className="col-md-4">
                        <div className="border rounded p-3 d-flex flex-column">
                            <h5 className="mb-3">Параметры обработки</h5>

                            <div className="mb-3">
                                <label className="form-label mb-2">Уменьшить разрешение</label>
                                <div className="d-flex flex-wrap gap-2">
                                    {resizeOptions.map((v) => {
                                        const optValue = v === 0 ? null : v;
                                        const label = v === 0 ? "—" : `${v}%`;
                                        return (
                                            <Button
                                                key={String(v)}
                                                variant={resize === optValue ? "primary" : "outline-secondary"}
                                                onClick={() => setResize(optValue)}
                                                className="flex-fill"
                                                style={{ minWidth: 88 }}
                                            >
                                                {label}
                                            </Button>
                                        );
                                    })}
                                </div>
                            </div>

                            <div className="mb-3">
                                <label className="form-label mb-2">Уменьшить вес</label>
                                <div className="d-flex flex-wrap gap-2">
                                    {compressOptions.map((v) => {
                                        const optValue = v === 0 ? null : v;
                                        const label = v === 0 ? "—" : `${v}%`;
                                        return (
                                            <Button
                                                key={String(v)}
                                                variant={compress === optValue ? "primary" : "outline-secondary"}
                                                onClick={() => setCompress(optValue)}
                                                className="flex-fill"
                                                style={{ minWidth: 88 }}
                                            >
                                                {label}
                                            </Button>
                                        );
                                    })}
                                </div>
                            </div>

                            <div className="mb-3">
                                <label className="form-label mb-2">Удалить метаданные</label>
                                {isAuthorized ? (
                                    <div className="d-flex flex-wrap gap-2">
                                        {removeEXIFOptions.map((v) => (
                                            <Button
                                                key={String(v)}
                                                variant={removeEXIF === v ? "primary" : "outline-secondary"}
                                                onClick={() => setRemoveEXIF(v)}
                                                className="flex-fill"
                                                style={{ minWidth: 88 }}
                                            >
                                                {v ? "Да" : "Нет"}
                                            </Button>
                                        ))}
                                    </div>
                                ) : (
                                    <Button
                                        variant="secondary"
                                        className="w-100"
                                        onMouseEnter={() => setRemoveHover(true)}
                                        onMouseLeave={() => setRemoveHover(false)}
                                        onClick={() => navigate("/auth")}
                                        title={removeHover ? "Перейти к регистрации" : "Доступно после регистрации"}
                                    >
                                        {removeHover ? "Зарегистрироваться?" : "Доступно после регистрации"}
                                    </Button>
                                )}
                            </div>

                            <div>
                                <Button
                                    variant="success"
                                    className="w-100"
                                    onClick={handleProcess}
                                    disabled={!uploads.length || isProcessing}
                                >
                                    {isProcessing ? "Обработка..." : "Начать обработку"}
                                </Button>
                            </div>
                        </div>
                    </div>
                </div>

                <Modal show={showModal} onHide={closeModal} centered backdrop="static">
                    <Modal.Header>
                        <Modal.Title>Обработка изображений</Modal.Title>
                        {!isProcessing && (
                            <button type="button" className="btn-close" aria-label="Close" onClick={closeModal} />
                        )}
                    </Modal.Header>
                    <Modal.Body>
                        <div className="mb-3">
                            <ProgressBar
                                now={overallProgress}
                                label={`${overallProgress}%`}
                                variant={archiveReady ? "success" : "primary"}
                            />
                        </div>

                        <div className="mb-2 text-center">
                            {archiveReady ? (
                                <span className="text-success">
                                    Обработка завершена! 
                                </span>
                            ) : (
                                <span>Обработано {processedCount} из {totalCount}</span>
                            )}
                        </div>

                        {archiveReady && downloadUrl && (
                            <>
                                <div className="d-grid gap-2 mb-3">
                                    <Button
                                        variant="primary"
                                        size="lg"
                                        onClick={() => {
                                            const sessionId = downloadUrl.split("/").pop();
                                            downloadArchive(sessionId!);
                                        }}
                                        disabled={downloadDisabled}
                                    >
                                        {downloadDisabled ? "Скачивание..." : "📥 Скачать архив"}
                                    </Button>
                                </div>
                                 {/*
                                <Form.Group className="mb-2">
                                    <Form.Label>Срок жизни ссылки</Form.Label>
                                    <Form.Control readOnly value={linkTTL ?? "Не указано"} />
                                </Form.Group>
                                */}
                            </>
                        )}
                    </Modal.Body>
                    <Modal.Footer>
                        <Button variant="outline-secondary" onClick={closeModal} disabled={isProcessing}>
                            {isProcessing ? "Обработка..." : "Закрыть"}
                        </Button>
                    </Modal.Footer>
                </Modal>
            </div>
    );
};

export default ModifierPage;