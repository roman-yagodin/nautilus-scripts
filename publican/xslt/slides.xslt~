 <xsl:stylesheet version="1.0" xmlns:xsl="http://www.w3.org/1999/XSL/Transform">

   <xsl:template match="/">
       <xsl:apply-templates select="@*|node()"/>
   </xsl:template>

   <!--all unconditioned sections, chapters, figures, examples, tables, equations, formalparas, notes, warnings, cautions must be in slides -->
   <xsl:template match="//section[not(@condition)]|//chapter[not(@condition)]|//figure[not(@condition)]|//example[not(@condition)]|//section/title[not(@condition)]|//chapter/title[not(@condition)]|//section/table[not(@condition)]|//section/equation[not(@condition)]|//section/formalpara[not(@condition)]|//section/note[not(@condition)]|//section/warning[not(@condition)]|//section/caution[not(@condition)]">
   <xsl:copy>
         <xsl:attribute name="condition">lesson,slides,print</xsl:attribute>
         <xsl:apply-templates select="@*|node()" />
       </xsl:copy>
   </xsl:template>

   <!--all other tags in section must be marked as not included in slides -->
   <xsl:template match="//section/*[not(@condition)]">
   <xsl:copy>
         <xsl:attribute name="condition">lesson,print</xsl:attribute>
         <xsl:apply-templates select="@*|node()" />
       </xsl:copy>
   </xsl:template>

   <!--Identity template copies content forward  -->
   <xsl:template match="@*|node()">
     <xsl:copy>
         <xsl:apply-templates select="@*|node()"/>
     </xsl:copy>
   </xsl:template>

</xsl:stylesheet>

