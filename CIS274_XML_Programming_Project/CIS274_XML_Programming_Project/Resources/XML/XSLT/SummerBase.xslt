<?xml version="1.0" encoding="utf-8"?>
<xsl:stylesheet
    version="1.0"
    xmlns:xsl="http://www.w3.org/1999/XSL/Transform"
    xmlns:msxsl="urn:schemas-microsoft-com:xslt" exclude-result-prefixes="msxsl"
>
    <xsl:output method="html" encoding="utf-8" indent="yes" />

    <xsl:template match="/">
      <!-- XSLT 1.0 doesn't seem to support HTML 5 doctype declaration; this is a kludge to fix that -->
      <xsl:text disable-output-escaping="yes">&lt;!DOCTYPE html&gt;&#xd;&#xa;</xsl:text>
      <html>
        <head>
          <title>Summer 2018 Course Schedule - Manchester Community College, New Hampshire</title>
          <meta name="viewport" content="width=device-width, initial-scale=1" />
        </head>
        <body>
          <table>
            <thead>
              <tr>
                <th title="CRN">CRN</th>
                <th title="Course">Course</th>
                <th title="Course Title">Course Title</th>
                <th title="CR">CR</th>
                <th title="Dates">Dates</th>
                <th title="Days">Days</th>
                <th title="Times">Times</th>
                <th title="Cost">Cost</th>
              </tr>
            </thead>
            <tbody>
              <xsl:for-each select="SheetSet/CourseSchedule/Class">
                <tr>
                  <td title="CRN">
                    <xsl:value-of select="CRN" />
                  </td>
                  <td title="Course">
                    <xsl:value-of select="concat(SUBJ, CRSE)" />
                  </td>
                  <td title="Course Title">
                    <xsl:value-of select="TITLE" />
                  </td>
                  <td title="CR">
                    <xsl:value-of select="CRDT_HRS" />
                  </td>
                  <td title="Dates">
                    <xsl:value-of select="concat(BEGINDATE, ' to ' , ENDDATE)" />
                  </td>
                  <td title="Days">
                    <xsl:value-of select="concat(MON, TUE, WED, THU, FRI, SAT)" />
                  </td>
                  <td title="Times">
                    <xsl:choose>
                      <xsl:when test="BEGIN != ''">
                        <xsl:value-of select="concat(BEGIN, ' to ', END)" />
                      </xsl:when>
                      <xsl:otherwise>Online</xsl:otherwise>
                    </xsl:choose>
                  </td>
                  <td title="Cost">
                    <xsl:choose>
                      <xsl:when test="COURSE_COST != '0'">
                        <xsl:value-of select="COURSE_COST" />
                      </xsl:when>
                      <xsl:otherwise></xsl:otherwise>
                    </xsl:choose>
                  </td>
                </tr>
              </xsl:for-each>
            </tbody>
          </table>
        </body>
      </html>
    </xsl:template>
</xsl:stylesheet>
