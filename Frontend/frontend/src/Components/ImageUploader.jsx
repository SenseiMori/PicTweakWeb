import React, { useState } from "react";

const ImageUploader = ({ onUploadSuccess }) => {
    const [images, setImages] = useState([]);
    const [isUploading, setIsUploading] = useState(false);
    const [uploadResults, setUploadResults] = useState([]);

    const handleFileChange = (event) => {
        const files = Array.from(event.target.files || []);
        const newImages = files.map((file) => ({
            file,
            previewUrl: URL.createObjectURL(file),
        }));
        setImages((prev) => [...prev, ...newImages]);
    };

    const removeImage = (index) => {
        setImages((prev) => {
            // Освобождаем память от URL объекта
            URL.revokeObjectURL(prev[index].previewUrl);
            return prev.filter((_, i) => i !== index);
        });
    };

    const getImageDimensions = (file) => {
        return new Promise((resolve) => {
            const img = new Image();
            img.onload = () => {
                resolve({ width: img.width, height: img.height });
                URL.revokeObjectURL(img.src); // Освобождаем память
            };
            img.onerror = () => {
                resolve({ width: 0, height: 0 });
                URL.revokeObjectURL(img.src); // Освобождаем память
            };
            img.src = URL.createObjectURL(file);
        });
    };

    const handleUpload = async () => {
        if (images.length === 0) {
            setUploadResults(["Нет изображений для загрузки."]);
            return;
        }

        setIsUploading(true);
        setUploadResults([]);
        const results = [];

        try {
            // Загружаем изображения по одному
            for (let i = 0; i < images.length; i++) {
                const img = images[i];

                try {
                    // Получаем размеры изображения
                    const dimensions = await getImageDimensions(img.file);

                    const formData = new FormData();
                    formData.append("JpegFile", img.file); // Соответствует JpegRequest.JpegFile
                    formData.append("FileName", img.file.name); // Соответствует JpegRequest.FileName
                    formData.append("Width", dimensions.width.toString()); // Соответствует JpegRequest.Width
                    formData.append("Height", dimensions.height.toString()); // Соответствует JpegRequest.Height
                    formData.append("Size", img.file.size.toString()); // Соответствует JpegRequest.Size

                    const response = await fetch("/api/images", {
                        method: "POST",
                        body: formData,
                    });

                    if (!response.ok) {
                        const errorText = await response.text();
                        throw new Error(`${response.status}: ${errorText}`);
                    }

                    const result = await response.json();
                    results.push(`✅ ${img.file.name}: успешно загружен (ID: ${result.id})`);

                } catch (error) {
                    results.push(`❌ ${img.file.name}: ${error.message}`);
                }
            }

            // Проверяем успешность загрузки
            const allSuccess = results.every(r => r.startsWith('✅'));
            const hasSuccess = results.some(r => r.startsWith('✅'));

            if (allSuccess) {
                // Все файлы загружены успешно - очищаем список
                images.forEach(img => URL.revokeObjectURL(img.previewUrl));
                setImages([]);

                // Вызываем callback для обновления родительского компонента
                if (onUploadSuccess) {
                    onUploadSuccess();
                }
            } else if (hasSuccess) {
                // Частичный успех - удаляем только успешно загруженные
                const successfulIndexes = [];
                results.forEach((result, index) => {
                    if (result.startsWith('✅')) {
                        successfulIndexes.push(index);
                    }
                });

                // Удаляем успешно загруженные изображения (в обратном порядке)
                successfulIndexes.reverse().forEach(index => {
                    URL.revokeObjectURL(images[index].previewUrl);
                });

                setImages(prev => prev.filter((_, index) => !successfulIndexes.includes(index)));

                // Вызываем callback, так как есть успешные загрузки
                if (onUploadSuccess) {
                    onUploadSuccess();
                }
            }

        } catch (error) {
            results.push(`Общая ошибка: ${error.message}`);
        } finally {
            setIsUploading(false);
            setUploadResults(results);
        }
    };

    // Очистка всех изображений
    const clearAllImages = () => {
        images.forEach(img => URL.revokeObjectURL(img.previewUrl));
        setImages([]);
        setUploadResults([]);
    };

    // Очистка URL объектов при размонтировании компонента
    React.useEffect(() => {
        return () => {
            images.forEach(img => URL.revokeObjectURL(img.previewUrl));
        };
    }, [images]);

    return (
        <div className="p-4 border rounded-lg shadow-md w-full max-w-xl mx-auto">
            <label className="block mb-2 text-lg font-medium text-gray-700">
                Выберите изображения (JPEG)
            </label>

            <input
                type="file"
                accept="image/jpeg,image/jpg" // Только JPEG файлы
                multiple
                onChange={handleFileChange}
                className="mb-4 block w-full text-sm text-gray-500
                          file:mr-4 file:py-2 file:px-4
                          file:rounded-full file:border-0
                          file:text-sm file:font-semibold
                          file:bg-blue-50 file:text-blue-700
                          hover:file:bg-blue-100"
                disabled={isUploading}
            />

            {images.length > 0 && (
                <div className="mb-4 flex justify-between items-center">
                    <p className="text-sm text-gray-600">
                        Выбрано файлов: {images.length}
                    </p>
                    <button
                        onClick={clearAllImages}
                        disabled={isUploading}
                        className="text-xs text-red-600 hover:text-red-800 
                                 disabled:opacity-50 disabled:cursor-not-allowed"
                    >
                        Очистить все
                    </button>
                </div>
            )}

            <div className="grid grid-cols-2 gap-4 mb-4">
                {images.map((img, idx) => (
                    <div
                        key={idx}
                        className="relative border rounded overflow-hidden shadow-sm"
                    >
                        <img
                            src={img.previewUrl}
                            alt={`preview-${idx}`}
                            className="w-full h-32 object-cover"
                        />
                        <div className="p-2">
                            <p className="text-xs text-gray-600 truncate">
                                {img.file.name}
                            </p>
                            <p className="text-xs text-gray-500">
                                {(img.file.size / 1024 / 1024).toFixed(2)} МБ
                            </p>
                        </div>
                        <button
                            onClick={() => removeImage(idx)}
                            disabled={isUploading}
                            className="absolute top-1 right-1 bg-red-600 text-white 
                                     rounded-full w-6 h-6 flex items-center justify-center
                                     hover:bg-red-700 transition-colors
                                     disabled:opacity-50 disabled:cursor-not-allowed"
                            type="button"
                            title="Удалить изображение"
                        >
                            ✕
                        </button>
                    </div>
                ))}
            </div>

            <button
                onClick={handleUpload}
                disabled={isUploading || images.length === 0}
                className="w-full bg-blue-600 text-white px-4 py-2 rounded 
                         disabled:opacity-50 disabled:cursor-not-allowed
                         hover:bg-blue-700 transition-colors"
            >
                {isUploading ? (
                    <span className="flex items-center justify-center">
                        <svg className="animate-spin -ml-1 mr-3 h-5 w-5 text-white" xmlns="http://www.w3.org/2000/svg" fill="none" viewBox="0 0 24 24">
                            <circle className="opacity-25" cx="12" cy="12" r="10" stroke="currentColor" strokeWidth="4"></circle>
                            <path className="opacity-75" fill="currentColor" d="M4 12a8 8 0 018-8V0C5.373 0 0 5.373 0 12h4zm2 5.291A7.962 7.962 0 014 12H0c0 3.042 1.135 5.824 3 7.938l3-2.647z"></path>
                        </svg>
                        Загрузка...
                    </span>
                ) : (
                    `Загрузить ${images.length} ${images.length === 1 ? 'изображение' : 'изображений'}`
                )}
            </button>

            {uploadResults.length > 0 && (
                <div className="mt-4 p-3 bg-gray-50 rounded">
                    <div className="flex justify-between items-center mb-2">
                        <h4 className="font-medium">Результаты загрузки:</h4>
                        <button
                            onClick={() => setUploadResults([])}
                            className="text-xs text-gray-500 hover:text-gray-700"
                        >
                            Очистить
                        </button>
                    </div>
                    <ul className="text-sm space-y-1">
                        {uploadResults.map((result, idx) => (
                            <li
                                key={idx}
                                className={result.startsWith('✅') ? 'text-green-600' : 'text-red-600'}
                            >
                                {result}
                            </li>
                        ))}
                    </ul>
                </div>
            )}
        </div>
    );
};

export default ImageUploader;
