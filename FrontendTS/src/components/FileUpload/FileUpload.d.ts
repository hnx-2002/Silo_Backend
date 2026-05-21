export interface FileData {
  file_id: string;
  file_name: string;
  file_length: number | undefined;
  file_ext: string | undefined;
  content: File | null | undefined;
  file_fullpath?: string;
}

export interface BatchUploadRow {
  file_id: string;
  file_name: string;
  file_length: number | undefined;
  file_ext: string | undefined;
  content: File | null | undefined;
  file_fullpath?: string;
}

export interface Res_UploadFile {
  status: boolean;
  filePath: string;
  msg: string;
  md5: string;
}
