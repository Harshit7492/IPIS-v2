/* ============================================================================
   SUPERSEDED - this file is intentionally empty and can be deleted.

   Its two methods now live in DataBaseHandler\_dbService.cs and call stored
   procedures instead of inline SQL:

       GetReportData(..)  ->  _dbService.GetLedMatrixReport(..)
                              calls USP_GET_LEDMATRIX_REPORT
       SyncFromApiLog(..) ->  _dbService.SyncLedMatrixFromApiLog(..)
                              calls USP_SYNC_LEDMATRIX_FROM_APILOG

   Both procedures are defined in LEDMatrix_Procedures.sql at the repo root.
   ============================================================================ */
