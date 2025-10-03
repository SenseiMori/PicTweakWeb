import React, { useState } from 'react';
import ImageUploader from './ImageUploader';

const ControlPanel = ({ onRefresh, onUploadSuccess }) => {
    const [showUploader, setShowUploader] = useState(false);

    return (
        <div className="p-4 border rounded bg-white shadow flex flex-col gap-2">
            <button onClick={() => setShowUploader(true)} className="bg-green-500 text-white px-2 py-1">Добавить</button>
            <button onClick={onRefresh} className="bg-blue-500 text-white px-2 py-1">Обновить</button>
            {/* Удаление обрабатывается в таблице; если нужно глобальное, добавьте */}
            <ImageUploader onUploadSuccess={onUploadSuccess} isVisible={showUploader} />
        </div>
    );
};

export default React.memo(ControlPanel);
