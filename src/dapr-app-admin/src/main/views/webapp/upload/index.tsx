import { connect } from 'react-redux';
import React from 'react';
import { InboxOutlined } from '@ant-design/icons';
import type { UploadProps } from 'antd';
import { message, Upload } from 'antd';
import { useAuth } from 'oidc-react';

const { Dragger } = Upload;

const WebappUpload: React.FC = () => {

    const auth = useAuth();

    const props: UploadProps = {
        name: 'file',
        multiple: false,
        maxCount: 1,
        action: '/admin/api/webapp/upload',
        headers: {
            Authorization: (auth && auth.userData && auth.userData.access_token) ? `Bearer ${auth.userData.access_token}` : '',
        },
        onChange(info) {
            const { status } = info.file;
            if (status !== 'uploading') {
                console.log(info.file, info.fileList);
            }
            if (status === 'done') {
                message.success(`${info.file.name} file uploaded successfully.`);
            } else if (status === 'error') {
                message.error(`${info.file.name} file upload failed.`);
            }
        },
        onDrop(e) {
            console.log('Dropped files', e.dataTransfer.files);
        },
    };

    return (
        <div style={{ height: '90%', padding: '0.5rem' }}>
            <Dragger {...props}>
                <p className="ant-upload-drag-icon">
                    <InboxOutlined />
                </p>
                <p className="ant-upload-text">Click or drag file to this area to upload</p>
                <p className="ant-upload-hint">
                    Support for a single upload. Strictly prohibited from uploading company data or other
                    banned files.
                </p>
            </Dragger>
        </div>
    );
}

const mapStateToProps = (state: any) => {
    return {
        ...state.app,
        ...state.settings
    }
}

export default connect(mapStateToProps)(WebappUpload)